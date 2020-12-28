using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VHacksWebstore.Core.Domain;
using VHacksWebstore.Data;

namespace VHacksWebstore.Core.App.Pages.Webstore.Products.Reviews
{
    public class EditModel : PageModel
    {
        private readonly VHacksWebstore.Data.WebstoreDbContext _context;

        public EditModel(VHacksWebstore.Data.WebstoreDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductRating ProductRating { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductRating = await _context.Ratings.FirstOrDefaultAsync(m => m.Id == id);

            if (ProductRating == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ProductRating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductRatingExists(ProductRating.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductRatingExists(string id)
        {
            return _context.Ratings.Any(e => e.Id == id);
        }
    }
}
