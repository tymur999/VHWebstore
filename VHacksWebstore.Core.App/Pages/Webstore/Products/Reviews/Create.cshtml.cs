using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VHacksWebstore.Core.Domain;
using VHacksWebstore.Data;

namespace VHacksWebstore.Core.App.Pages.Webstore.Products.Reviews
{
    public class CreateModel : PageModel
    {
        private readonly VHacksWebstore.Data.WebstoreDbContext _context;

        public CreateModel(VHacksWebstore.Data.WebstoreDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProductRating ProductRating { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Ratings.Add(ProductRating);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
