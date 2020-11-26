using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace VHacksWebstore.Core.Domain
{
    public class WebstoreUser : IdentityUser
    {
        public ICollection<Product> RecommendedProducts { get; set; }
        public ICollection<ProductOrder> Orders { get; set; }
        public ICollection<ProductRating> Ratings { get; set; }
    }
}
