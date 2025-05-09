using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.DTOs
{
    public class ProductImageDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required(ErrorMessage = "Image URL is required.")]
        [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters.")]
        public string ImageUrl { get; set; }

        public bool IsMainImage { get; set; } = false;

        public bool IsDeleted { get; set; } = false;  // Soft delete flag

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}