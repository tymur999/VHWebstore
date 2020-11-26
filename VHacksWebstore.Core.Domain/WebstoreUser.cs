using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace VHacksWebstore.Core.Domain
{
    public class WebstoreUser : IdentityUser
    {
        public IEnumerable<Product> RecommendedProducts { get; set; }
        public IEnumerable<ProductOrder> Orders { get; set; }
        public IEnumerable<ProductRating> Ratings { get; set; }
    }
}
