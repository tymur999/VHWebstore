using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VHacksWebstore.Core.Domain
{
    public class ProductRating
    {
        [Key]
        public string Id { get; set; }
        public WebstoreUser User { get; set; }
        public Product Product { get; set; }
        [Range(1,5)]
        public int Rating { get; set; }
        [Range(0, 100)]
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public IList<Image> Images { get; set; }
    }
}
