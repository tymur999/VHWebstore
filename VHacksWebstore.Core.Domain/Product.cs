using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;


namespace VHacksWebstore.Core.Domain
{
    public class Product
    {
        [Key] //Guid
        public string Id { get; set; }
        //List of file paths in json format
        public List<string> Images { get; set; } = new();
        public List<ProductOrder> Orders { get; set; } = new();
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public float Rating { get; set; }
        public List<ProductRating> Reviews { get; set; }
    }
}
