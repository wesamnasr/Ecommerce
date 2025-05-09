using WebApplicationTest.DTOs;
using WebApplicationTest.Mapper;
using WebApplicationTest.Models;

public static class ProductMapper
{
    public static ProductDto ToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            MainImageUrl = product.MainImageUrl,
            CategoryId = product.CategoryId,
            IsDeleted = product.IsDeleted,
            CreatedDate = product.CreatedDate,
            UpdatedDate = product.UpdatedDate,
            CreatedBy = product.CreatedBy,
            UpdatedBy = product.UpdatedBy,
            Images = product.Images?
                .Where(pi => !pi.IsDeleted)
                .Select(pi => new ProductImageDto
                {
                    Id = pi.Id,
                    ProductId = pi.ProductId,
                    ImageUrl = pi.ImageUrl,
                    IsMainImage = pi.IsMainImage,
                    IsDeleted = pi.IsDeleted,
                    CreatedDate = pi.CreatedDate,
                    UpdatedDate = pi.UpdatedDate,
                    CreatedBy = pi.CreatedBy,
                    UpdatedBy = pi.UpdatedBy
                })
                .ToList(),
            Category = product.Category != null ? CategoryMapper.ToDto(product.Category) : null
        };
    }

    public static Product ToEntity(ProductDto productDto, string createdBy, string updatedBy)
    {
        return new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            Stock = productDto.Stock,
            MainImageUrl = productDto.MainImageUrl,
            CategoryId = productDto.CategoryId,
            IsDeleted = productDto.IsDeleted,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            CreatedBy = createdBy,
            UpdatedBy = updatedBy,
            Images = productDto.Images?
                .Select(pi => new ProductImage
                {
                    ImageUrl = pi.ImageUrl,
                    IsMainImage = pi.IsMainImage,
                    IsDeleted = pi.IsDeleted,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    CreatedBy = updatedBy,
                    UpdatedBy = updatedBy
                })
                .ToList()
        };
    }
}
