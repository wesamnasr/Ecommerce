using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Models
{
    public class Category
    {
        public int Id { get; set; }  // Primary key

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; }  // Category name

        [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters.")]
        public string? Image { get; set; } // Category image URL

        public bool IsDeleted { get; set; } = false; // Soft delete flag
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Auto-set creation date
        public DateTime? UpdatedDate { get; set; }  // Nullable update date
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }

        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}