using System.Collections.Generic;
using WebApplicationTest.DTOs;

namespace WebApplicationTest.Services
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetAllCategories();
        CategoryDto? GetCategoryById(int id);
        void AddCategory(CategoryDto category, string createdBy, string updatedBy);
        void UpdateCategory(CategoryDto category, string updatedBy);
        void SoftDeleteCategory(int id, string updatedBy);
    }
}