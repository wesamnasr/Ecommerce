using System.Collections.Generic;
using WebApplicationTest.DTOs;

namespace WebApplicationTest.ViewModels
{
    public class ProductViewModel
    {
        public ProductDto Product { get; set; }
        public string? MainImageUrl { get; set; }
        public List<string> AdditionalImageUrls { get; set; } = new List<string>();
    }
}