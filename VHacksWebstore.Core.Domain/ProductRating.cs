using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VHacksWebstore.Core.Domain
{
    public class ProductRating
    {
        [Key]
        public string Id { get; set; }
        public IdentityUser User { get; set; }
        public Product Product { get; set; }
        [Range(1,5)]
        public int Rating { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}
