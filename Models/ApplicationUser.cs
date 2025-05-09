using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(100)]
        public string FullName { get; set; }  // Full name of the user

        [StringLength(255)]
        public string Address { get; set; }  // Shipping address

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        public bool IsActive { get; set; } = true; // To disable user accounts if needed

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // New Field
    }
}
