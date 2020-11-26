using System.ComponentModel.DataAnnotations;

namespace VHacksWebstore.Core.Domain
{
    public class TopRatedProduct
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public Product Product { get; set; }
    }
}
