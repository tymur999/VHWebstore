using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VHacksWebstore.Core.Domain;
using VHacksWebstore.Data;

namespace VHacksWebstore.Core.App.Pages.Webstore.Products.Reviews
{
    public class DetailsModel : PageModel
    {
        private readonly VHacksWebstore.Data.WebstoreDbContext _context;

        public DetailsModel(VHacksWebstore.Data.WebstoreDbContext context)
        {
            _context = context;
        }

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
    }
}
