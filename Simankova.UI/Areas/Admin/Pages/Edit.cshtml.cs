using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Simankova.Domain.Entities;
using IProductService = Simankova.UI.Interfaces.IProductService;

namespace Simankova.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService productService;

        public EditModel(IProductService productService)
        {
            this.productService = productService;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // if (id == null)
            // {
            //     return NotFound();
            // }

            // var product =  await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            // if (product == null)
            // {
            //     return NotFound();
            // }
            // Product = product;
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Attach(Product).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ProductExists(Product.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return true;
            //return _context.Products.Any(e => e.Id == id);
        }
    }
}
