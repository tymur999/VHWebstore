using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace VHacksWebstore.Core.Domain
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public byte[] PrimaryImage { get; set; }
        //Separate byte arrays with ','
        public string Images
        {
            get => Images;
            set => string.Join(',', value);
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        [Range(1,5)]
        public float Rating { get; set; }
        public ICollection<ProductRating> Reviews { get; set; }
    }
}
