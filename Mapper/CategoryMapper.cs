using WebApplicationTest.DTOs;
using WebApplicationTest.Models;

namespace WebApplicationTest.Mapper
{
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.Image,
                IsDeleted = category.IsDeleted,
                CreatedDate = category.CreatedDate,
                UpdatedDate = category.UpdatedDate,
                CreatedBy = category.CreatedBy,
                UpdatedBy = category.UpdatedBy,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategory = category.ParentCategory != null ? ToDto(category.ParentCategory) : null,
                SubCategories = category.SubCategories?.Where(sc => !sc.IsDeleted).Select(ToDto).ToList(),
                Products = category.Products?.Where(p => !p.IsDeleted).Select(ProductMapper.ToDto).ToList()
            };
        }
        public static Category ToEntity(this CategoryDto categoryDto, string createdBy)
        {
            return new Category
            {
                Name = categoryDto.Name,
                Image = categoryDto.Image,
                ParentCategoryId = categoryDto.ParentCategoryId,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = createdBy,
              
               
                    
            };
        }
    }

}
