using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }  // Primary key

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }  // Foreign key to ApplicationUser

        public virtual ApplicationUser User { get; set; }  // Navigation property

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }  // Foreign key to Product

        public virtual Product Product { get; set; }  // Navigation property

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }  // Quantity of the product in the cart
    }
}
