using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using VHacksWebstore.Core.Domain;
using VHacksWebstore.Data;

namespace VHacksWebstore.Core.App.Pages.Webstore.Products
{
    public class IndexModel : PageModel
    {
        private readonly WebstoreDbContext _context;
        private readonly IMemoryCache _cache;

        public IndexModel(WebstoreDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        [BindProperty]
        public List<Product> Products { get; set; }
        [FromQuery(Name="search")]
        public string Search { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            //If the user is searching, try to find that product
            if (Search != null)
            {
                Products = _context.Products.Where(
                        x => x.Name.ToLower().Contains(Search.ToLower()) ||
                              x.Description.ToLower().Contains(Search.ToLower()))
                    .ToList();
                if (Products.Count == 0)
                {   
                    var productFromId = await _context.Products.FindAsync(Search.ToLower());
                    if(productFromId != null) Products.Add(productFromId);
                }
                return Page();
            }
            
            if (!_cache.TryGetValue("products", out var products))
            {
                var newProducts = await _context.Products.ToListAsync();
                _cache.Set("products", newProducts);
                Products = newProducts;
            }
            else
            {
                Products = (List<Product>)products;
            }

            return Page();
        }
    }
}
