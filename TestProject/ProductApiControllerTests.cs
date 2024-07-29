using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Simankova.Api.Controllers;
using Simankova.Api.Data;
using Simankova.Domain.Entities;
using Simankova.Domain.Models;

namespace TestProject;

public class ProductApiControllerTests : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
    private readonly IWebHostEnvironment _environment;
    public ProductApiControllerTests()
    {
        _environment = Substitute.For<IWebHostEnvironment>();
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite,including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;
        // Create the schema and seed some data
        using var context = new ApplicationDbContext(_contextOptions);
        context.Database.EnsureCreated();
        var categories = new Category[]
        {
 new Category {Name="", NormalizedName="soups"},
 new Category {Name="", NormalizedName="main-dishes"}
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();
        var dishes = new List<Product>
        {
             new Product {Name="", Description="", Price=0,
                Category=categories
             .FirstOrDefault(c=>c.NormalizedName.Equals("soups"))},
             new Product {Name = "", Description="", Price=0,
                Category=categories
             .FirstOrDefault(c=>c.NormalizedName.Equals("soups"))},
             new Product {Name = "", Description="", Price=0,
                Category=categories
             .FirstOrDefault(c=>c.NormalizedName.Equals("main-dishes"))},
             new Product {Name = "", Description="", Price=0,
                Category=categories
             .FirstOrDefault(c=>c.NormalizedName.Equals("main-dishes"))},
             new Product {Name = "", Description="", Price=0,
                Category=categories
             .FirstOrDefault(c=>c.NormalizedName.Equals("main-dishes"))}
        };
        context.AddRange(dishes);
        context.SaveChanges();
    }
    public void Dispose() => _connection?.Dispose();
    ApplicationDbContext CreateContext() => new ApplicationDbContext(_contextOptions);
    // Проверка фильтра по категории
    [Fact]
    public async void ControllerFiltersCategory()
    {
        // arrange
        using var context = CreateContext();
        var category = context.Categories.First();
        var controller = new ProductsController(context, _environment);
        // act
        var response = await controller.GetProducts(category.NormalizedName);
        ResponseData<ProductListModel<Product>> responseData = response.Value;
        var dishesList = responseData.Data.Items; // полученный список объектов
                                                  //assert
        Assert.True(dishesList.All(d => d.CategoryId == category.Id));
    }
    // Проверка подсчета количества страниц
    // Первый параметр - размер страницы
    // Второй параметр - ожидаемое количество страниц (при условии, что всегообъектов 5)
    [Theory]
    [InlineData(2, 3)]
    [InlineData(3, 2)]
    public async void ControllerReturnsCorrectPagesCount(int size, int qty)
    {
        using var context = CreateContext();
        var controller = new ProductsController(context, _environment);
        // act
        var response = await controller.GetProducts(null, 1, size);
        ResponseData<ProductListModel<Product>> responseData = response.Value;
        var totalPages = responseData.Data.TotalPages; // полученное количествостраниц
        //assert
        Assert.Equal(qty, totalPages); // количество страниц совпадает
    }
    [Fact]
    public async void ControllerReturnsCorrectPage()
    {
        using var context = CreateContext();
        var controller = new ProductsController(context, _environment);
        // При размере страницы 3 и общем количестве объектов 5
        // на 2-й странице должно быть 2 объекта
        int itemsInPage = 2;
        // Первый объект на второй странице
        Product firstItem = context.Products.ToArray()[3];
        // act
        // Получить данные 2-й страницы
        var response = await controller.GetProducts(null, 2);
        ResponseData<ProductListModel<Product>> responseData = response.Value;
        var dishesList = responseData.Data.Items; // полученный список объектов
        var currentPage = responseData.Data.CurrentPage; // полученный номер текущейстраницы
        //assert
        Assert.Equal(2, currentPage);// номер страницы совпадает
        Assert.Equal(2, dishesList.Count); // количество объектов на странице равно2
        Assert.Equal(firstItem.Id, dishesList[0].Id); // 1-й объект в спискеправильный
    }
}