using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VHacksWebstore.Core.Domain;
using VHacksWebstore.Data;

namespace VHacksWebstore.Core.App.Pages.Webstore.Products
{
    public class ProductModel : PageModel
    {
        private readonly WebstoreDbContext _context;

        public ProductModel(WebstoreDbContext context)
        {
            _context = context;
        }
        [FromRoute(Name = "productId")]
        public string Id { get; set; }
        public Product Product { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null) return RedirectToPage("./Index");
            Product = product;
            return Page();
        }
    }
}
