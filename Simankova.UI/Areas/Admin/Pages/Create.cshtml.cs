﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Simankova.Domain.Entities;
using Simankova.UI.Interfaces;

namespace Simankova.UI.Areas.Admin.Pages;
public class CreateModel(ICategoryService categoryService, IProductService
    productService) : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        var categoryListData = await categoryService.GetCategoryListAsync();
        ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id",
            "Name");
        return Page();
    }
    [BindProperty]
    public Product Product { get; set; } = default!;
    [BindProperty]
    public IFormFile? Image { get; set; }
    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        await productService.CreateProductAsync(Product, Image);
        return RedirectToPage("./Index");
    }
}

