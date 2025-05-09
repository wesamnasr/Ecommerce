using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Models
{
    public class Order
    {
        public int Id { get; set; }  // Primary key

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }  // Foreign key to ApplicationUser

        public virtual ApplicationUser User { get; set; } // Navigation property

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;  // Auto-set order date

        public OrderStatus Status { get; set; } = OrderStatus.Pending; // Default status

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0.")]
        public decimal TotalAmount { get; set; } = 0.00m; // Default value to prevent errors

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Track record creation
        public DateTime? UpdatedDate { get; set; } // Track updates

        // Navigation property for order details
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}
