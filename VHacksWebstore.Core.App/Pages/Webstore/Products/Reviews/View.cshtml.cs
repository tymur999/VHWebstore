using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VHacksWebstore.Core.Domain;
using VHacksWebstore.Data;

namespace VHacksWebstore.Core.App.Pages.Webstore.Products.Reviews
{
    public class ViewModel : PageModel
    {
        private readonly WebstoreDbContext _context;

        public ViewModel(WebstoreDbContext context)
        {
            _context = context;
        }
        public ProductRating Review { get; set; }
        [FromRoute(Name = "reviewId")]
        public string ReviewId { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var review = await _context.Ratings.FindAsync(ReviewId);
            if (review == null)
            {
                return Redirect("~/Webstore/Products");
            }

            Review = review;
            return Page();
        }
    }
}
