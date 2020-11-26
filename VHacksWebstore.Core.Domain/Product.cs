using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VHacksWebstore.Core.Domain
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public IList<Image> Images { get; set; }
        //Add collection of images later on
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        [Range(1,5)]
        public float Rating { get; set; }
        public IEnumerable<ProductRating> Ratings { get; set; }
    }
}
