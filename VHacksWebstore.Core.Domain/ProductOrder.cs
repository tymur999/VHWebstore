using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHacksWebstore.Core.Domain
{
    public class ProductOrder
    {
        [Key]
        public string Id { get; set; }
        public WebstoreUser Buyer { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
        public string Comment { get; set; }
        public OrderStatus Status { get; set; }
        public enum OrderStatus
        {
            Cancelled,
            [Display(Name = "In review")]
            Reviewing,
            [Display(Name ="In progress")]
            InProgress,
            Completed
        }
    }
}
