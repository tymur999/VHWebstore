using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VHacksWebstore.Core.Domain;
using VHacksWebstore.Data;

namespace VHacksWebstore.Core.App.Pages.Webstore.Products.Reviews
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly WebstoreDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(WebstoreDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty, Required] public ProductRating Rating { get; set; } = new();
        [FromRoute(Name = "productId")]
        public string ProductId { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == ProductId);
            if (product == null)
            {
                return Redirect("~/Webstore/Products");
            }

            Rating.User = await _userManager.GetUserAsync(User);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Rating.Id = Guid.NewGuid().ToString();
            await _context.Ratings.AddAsync(Rating);
            await _context.SaveChangesAsync();
            return RedirectToPage("./View");
        }
    }
}
