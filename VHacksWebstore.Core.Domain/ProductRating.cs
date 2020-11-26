using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

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
        public string Description { get; set; }
        //add a collection later on
        public byte[] Images { get; set; }
    }
}
