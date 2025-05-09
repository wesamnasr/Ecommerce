using System.Collections.Generic;
using WebApplicationTest.DTOs;

namespace WebApplicationTest.Services
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetAllProducts();
        ProductDto? GetProductById(int id);
        void AddProduct(ProductDto product, string createdBy, string updatedBy);
        void UpdateProduct(ProductDto product, string updatedBy);
        void SoftDeleteProduct(int id, string updatedBy);
    }
}