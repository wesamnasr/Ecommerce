using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Models
{
    public class ProductImage
    {
        public int Id { get; set; }  // Primary key

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }  // Foreign key to Product

        [Required(ErrorMessage = "Image URL is required.")]
        [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters.")]
        public string ImageUrl { get; set; }  // URL or file path of the image

        public bool IsMainImage { get; set; } = false;  // To identify the main image

        public bool IsDeleted { get; set; } = false;  // Soft delete flag

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Navigation property to the parent product
        public virtual Product Product { get; set; }
    }
}