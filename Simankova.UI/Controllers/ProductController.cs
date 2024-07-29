using Microsoft.AspNetCore.Mvc;
using ICategoryService = Simankova.UI.Interfaces.ICategoryService;
using IProductService = Simankova.UI.Interfaces.IProductService;

namespace Simankova.UI.Controllers
{
    public class ProductController(ICategoryService categoryService, IProductService productService) : Controller
    {
        public async Task<IActionResult> Index(string? category, int? pageno)
        {
            // получить список категорий
            var categoriesResponse = await
            categoryService.GetCategoryListAsync();
            // если список не получен, вернуть код 404
            if (!categoriesResponse.Success)
                return NotFound(categoriesResponse.ErrorMessage);
            // передать список категорий во ViewData
            ViewData["categories"] = categoriesResponse.Data;
            // передать во ViewData имя текущей категории
            var currentCategory = categoriesResponse.Data.FirstOrDefault(c =>
            c.NormalizedName == category);

            ViewData["currentCategory"] = currentCategory;

            var productResponse = await productService.GetProductListAsync(category, pageno.GetValueOrDefault(1));
            if (!productResponse.Success)
                ViewData["Error"] = productResponse.ErrorMessage;

            
            return View(productResponse.Data);
        }
    }
}
