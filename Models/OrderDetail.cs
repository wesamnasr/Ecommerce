using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }  // Primary key

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }  // Foreign key to Order
        public virtual Order Order { get; set; }  // Navigation property

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }  // Foreign key to Product
        public virtual Product Product { get; set; }  // Navigation property

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }  // Quantity of the product

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }  // Price at the time of purchase
    }
}
