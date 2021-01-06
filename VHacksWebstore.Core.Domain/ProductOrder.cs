using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VHacksWebstore.Core.Domain
{
    public class ProductOrder
    {
        [Key] //Guid
        public string Id { get; set; }
        public IdentityUser Buyer { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
        public string Comment { get; set; }
        public OrderStatus Status { get; set; }
        public enum OrderStatus
        {
            Cancelled,
            [Display(Name = "In review")]
            InReview,
            [Display(Name ="In progress")]
            InProgress,
            Completed
        }
    }
}
