﻿namespace WebApplicationTest.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    }

}
