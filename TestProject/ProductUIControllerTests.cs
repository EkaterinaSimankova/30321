using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Simankova.Domain.Entities;
using Simankova.Domain.Models;
using Simankova.UI.Controllers;
using Simankova.UI.Interfaces;

namespace TestProject;

public class ProductUIControllerTests
{
    IProductService _productService;
    ICategoryService _categoryService;
    public ProductUIControllerTests()
    {
        SetupData();
    }
    // Список категорий сохраняется во ViewData
    [Fact]
    public async void IndexPutsCategoriesToViewData()
    {
        //arrange
        var controller = new ProductController(_categoryService, _productService);
        //act
        var response = await controller.Index(null, null);
        //assert
        var view = Assert.IsType<ViewResult>(response);
        var categories = Assert.IsType<List<Category>>(view.ViewData["categories"]);
        Assert.Equal(6, categories.Count);
        Assert.Equal(null, view.ViewData["currentCategory"]);
    }
    // Имя текущей категории сохраняется во ViewData
    [Fact]
    public async void IndexSetsCorrectCurrentCategory()
    {
        //arrange
        var categories = await _categoryService.GetCategoryListAsync();
        var currentCategory = categories.Data[0];
        var controller = new ProductController(_categoryService, _productService);
        //act
        var response = await controller.Index(currentCategory.NormalizedName, null);
        //assert
        var view = Assert.IsType<ViewResult>(response);

        Assert.Equal(currentCategory.Name, (view.ViewData["currentCategory"] as Category).Name);
    }
    // В случае ошибки возвращается NotFoundObjectResult
    [Fact]
    public async void IndexReturnsNotFound()
    {
        //arrange
        string errorMessage = "Test error";
        var categoriesResponse = new ResponseData<List<Category>>();
        categoriesResponse.Success = false;
        categoriesResponse.ErrorMessage = errorMessage;

        _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse))
            ;
        var controller = new ProductController(_categoryService, _productService);
        //act
        var response = await controller.Index(null, null);
        //assert
        var result = Assert.IsType<NotFoundObjectResult>(response);
        Assert.Equal(errorMessage, result.Value.ToString());
    }
    // Настройка имитации ICategoryService и IProductService
    void SetupData()
    {
        _categoryService = Substitute.For<ICategoryService>();
        var categoriesResponse = new ResponseData<List<Category>>();
        categoriesResponse.Data = new List<Category>
        {
            new Category {Id=1, Name="Стартеры", NormalizedName="starters"},
            new Category {Id=2, Name="Салаты", NormalizedName="salads"},
            new Category {Id=3, Name="Супы", NormalizedName="soups"},
            new Category {Id=4, Name="Основные блюда", NormalizedName="maindishes"},
            new Category {Id=5, Name="Напитки", NormalizedName="drinks"},
            new Category {Id=6, Name="Десерты", NormalizedName="desserts"}
        };

        _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse))
            ;
        _productService = Substitute.For<IProductService>();
        var dishes = new List<Product>
        {
            new Product {Id = 1 },
            new Product { Id = 2 },
            new Product { Id = 3 },
            new Product { Id = 4 },
            new Product { Id = 5 }
        };
        var productResponse = new ResponseData<ProductListModel<Product>>();
        productResponse.Data = new ProductListModel<Product> { Items = dishes };
        _productService.GetProductListAsync(Arg.Any<string?>(), Arg.Any<int>())
            .Returns(productResponse);
    }
}