using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }  // 1 to 5 stars

        [Required]
        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    }

}
