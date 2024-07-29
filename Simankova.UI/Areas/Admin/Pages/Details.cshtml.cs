using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Simankova.Domain.Entities;
using IProductService = Simankova.UI.Interfaces.IProductService;

namespace Simankova.UI.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService productService;

        public DetailsModel(IProductService productService)
        {
            this.productService = productService;
        }

        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var product = await productService.FirstOrDefaultAsync(m => m.Id == id);
            //if (product == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    Product = product;
            //}
            return Page();
        }
    }
}
