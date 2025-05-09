using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using WebApplicationTest.DTOs;
using WebApplicationTest.Models;

namespace WebApplicationTest.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductDto> GetAllProducts()
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => !p.IsDeleted)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    MainImageUrl = p.MainImageUrl,
                    CategoryId = p.CategoryId,
                    IsDeleted = p.IsDeleted,
                    CreatedDate = p.CreatedDate,
                    UpdatedDate = p.UpdatedDate,
                    CreatedBy = p.CreatedBy,
                    UpdatedBy = p.UpdatedBy,
                    Images = p.Images
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
                    Category = p.Category != null ? new CategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name,
                        Image = p.Category.Image,
                        IsDeleted = p.Category.IsDeleted,
                        CreatedDate = p.Category.CreatedDate,
                        UpdatedDate = p.Category.UpdatedDate,
                        CreatedBy = p.Category.CreatedBy,
                        UpdatedBy = p.Category.UpdatedBy,
                        ParentCategoryId = p.Category.ParentCategoryId
                    } : null
                })
                .ToList();
        }

        public ProductDto? GetProductById(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefault(p => p.Id == id && !p.IsDeleted);

            if (product == null)
            {
                return null;
            }

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
                Images = product.Images
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
                Category = product.Category != null ? new CategoryDto
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name,
                    Image = product.Category.Image,
                    IsDeleted = product.Category.IsDeleted,
                    CreatedDate = product.Category.CreatedDate,
                    UpdatedDate = product.Category.UpdatedDate,
                    CreatedBy = product.Category.CreatedBy,
                    UpdatedBy = product.Category.UpdatedBy,
                    ParentCategoryId = product.Category.ParentCategoryId
                } : null
            };
        }

        public void AddProduct(ProductDto productDto, string createdBy, string updatedBy)
        {
            var product = new Product
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
                Images = productDto.Images
                    .Select(pi => new ProductImage
                    {
                        Id = pi.Id,
                        ProductId = pi.ProductId,
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

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(ProductDto productDto, string updatedBy)
        {
            var product = _context.Products.Find(productDto.Id);
            if (product != null)
            {
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Price = productDto.Price;
                product.Stock = productDto.Stock;
                product.MainImageUrl = productDto.MainImageUrl;
                product.CategoryId = productDto.CategoryId;
                product.UpdatedDate = DateTime.UtcNow;
                product.UpdatedBy = updatedBy;

                // Update product images
                var existingImages = product.Images.ToList();
                var productImages = new List<ProductImage>();

                // Keep existing images
                foreach (var imageUrl in productDto.Images.Select(pi => pi.ImageUrl))
                {
                    var existingImage = existingImages.FirstOrDefault(i => i.ImageUrl == imageUrl);
                    if (existingImage != null)
                    {
                        productImages.Add(existingImage);
                    }
                }

                // Add new images
                foreach (var imageDto in productDto.Images.Where(pi => pi.Id == 0))
                {
                    productImages.Add(new ProductImage
                    {
                        ImageUrl = imageDto.ImageUrl,
                        IsMainImage = imageDto.IsMainImage,
                        IsDeleted = imageDto.IsDeleted,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = updatedBy,
                        UpdatedBy = updatedBy
                    });
                }

                product.Images = productImages;

                _context.Products.Update(product);
                _context.SaveChanges();
            }
        }

        public void SoftDeleteProduct(int id, string updatedBy)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                product.IsDeleted = true;
                product.UpdatedDate = DateTime.UtcNow;
                product.UpdatedBy = updatedBy;
                _context.Products.Update(product);
                _context.SaveChanges();
            }
        }
    }
}