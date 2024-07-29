using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Simankova.Domain.Entities;
using IProductService = Simankova.UI.Interfaces.IProductService;

namespace Simankova.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService productService;

        public DeleteModel(IProductService productService)
        {
            this.productService = productService;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var product = await _context.Products.FindAsync(id);
            //if (product != null)
            //{
            //    Product = product;
            //    _context.Products.Remove(Product);
            //    await _context.SaveChangesAsync();
            //}

            return RedirectToPage("./Index");
        }
    }
}
