using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace VHacksWebstore.Core.Domain
{
    public class ProductRating
    {
        [Key] //Guid
        public string Id { get; set; }
        public IdentityUser User { get; set; }
        [Range(1,5)]
        public int Rating { get; set; }
        public Product ReviewProduct { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
    }
}
