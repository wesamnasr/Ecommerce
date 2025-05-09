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
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CategoryDto> GetAllCategories()
        {
            return _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.ParentCategory)
                .Where(c => !c.IsDeleted)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Image = c.Image,
                    IsDeleted = c.IsDeleted,
                    CreatedDate = c.CreatedDate,
                    UpdatedDate = c.UpdatedDate,
                    CreatedBy = c.CreatedBy,
                    UpdatedBy = c.UpdatedBy,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategory = c.ParentCategory != null ? new CategoryDto
                    {
                        Id = c.ParentCategory.Id,
                        Name = c.ParentCategory.Name,
                        Image = c.ParentCategory.Image,
                        IsDeleted = c.ParentCategory.IsDeleted,
                        CreatedDate = c.ParentCategory.CreatedDate,
                        UpdatedDate = c.ParentCategory.UpdatedDate,
                        CreatedBy = c.ParentCategory.CreatedBy,
                        UpdatedBy = c.ParentCategory.UpdatedBy,
                        ParentCategoryId = c.ParentCategory.ParentCategoryId
                    } : null,
                    SubCategories = c.SubCategories
                        .Where(sc => !sc.IsDeleted)
                        .Select(sc => new CategoryDto
                        {
                            Id = sc.Id,
                            Name = sc.Name,
                            Image = sc.Image,
                            IsDeleted = sc.IsDeleted,
                            CreatedDate = sc.CreatedDate,
                            UpdatedDate = sc.UpdatedDate,
                            CreatedBy = sc.CreatedBy,
                            UpdatedBy = sc.UpdatedBy,
                            ParentCategoryId = sc.ParentCategoryId
                        })
                        .ToList(),
                    Products = c.Products
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
                            Category = new CategoryDto
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Image = c.Image,
                                IsDeleted = c.IsDeleted,
                                CreatedDate = c.CreatedDate,
                                UpdatedDate = c.UpdatedDate,
                                CreatedBy = c.CreatedBy,
                                UpdatedBy = c.UpdatedBy,
                                ParentCategoryId = c.ParentCategoryId
                            }
                        })
                        .ToList()
                })
                .ToList();
        }

        public CategoryDto? GetCategoryById(int id)
        {
            var category = _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.ParentCategory)
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);

            if (category == null)
            {
                return null;
            }

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
                ParentCategory = category.ParentCategory != null ? new CategoryDto
                {
                    Id = category.ParentCategory.Id,
                    Name = category.ParentCategory.Name,
                    Image = category.ParentCategory.Image,
                    IsDeleted = category.ParentCategory.IsDeleted,
                    CreatedDate = category.ParentCategory.CreatedDate,
                    UpdatedDate = category.ParentCategory.UpdatedDate,
                    CreatedBy = category.ParentCategory.CreatedBy,
                    UpdatedBy = category.ParentCategory.UpdatedBy,
                    ParentCategoryId = category.ParentCategory.ParentCategoryId
                } : null,
                SubCategories = category.SubCategories
                    .Where(sc => !sc.IsDeleted)
                    .Select(sc => new CategoryDto
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        Image = sc.Image,
                        IsDeleted = sc.IsDeleted,
                        CreatedDate = sc.CreatedDate,
                        UpdatedDate = sc.UpdatedDate,
                        CreatedBy = sc.CreatedBy,
                        UpdatedBy = sc.UpdatedBy,
                        ParentCategoryId = sc.ParentCategoryId
                    })
                    .ToList(),
                Products = category.Products
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
                        Category = new CategoryDto
                        {
                            Id = category.Id,
                            Name = category.Name,
                            Image = category.Image,
                            IsDeleted = category.IsDeleted,
                            CreatedDate = category.CreatedDate,
                            UpdatedDate = category.UpdatedDate,
                            CreatedBy = category.CreatedBy,
                            UpdatedBy = category.UpdatedBy,
                            ParentCategoryId = category.ParentCategoryId
                        }
                    })
                    .ToList()
            };
        }

        public void AddCategory(CategoryDto categoryDto, string createdBy, string updatedBy)
        {
            var category = new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Image = categoryDto.Image,
                IsDeleted = categoryDto.IsDeleted,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = createdBy,
                UpdatedBy = updatedBy,
                ParentCategoryId = categoryDto.ParentCategoryId,
                SubCategories = categoryDto.SubCategories
                    .Select(sc => new Category
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        Image = sc.Image,
                        IsDeleted = sc.IsDeleted,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                        CreatedBy = updatedBy,
                        UpdatedBy = updatedBy,
                        ParentCategoryId = sc.ParentCategoryId
                    })
                    .ToList(),
                Products = categoryDto.Products
                    .Select(p => new Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Stock = p.Stock,
                        MainImageUrl = p.MainImageUrl,
                        CategoryId = p.CategoryId,
                        IsDeleted = p.IsDeleted,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                        CreatedBy = updatedBy,
                        UpdatedBy = updatedBy,
                        Images = p.Images
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
                    })
                    .ToList()
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(CategoryDto categoryDto, string updatedBy)
        {
            var category = _context.Categories.Find(categoryDto.Id);
            if (category != null)
            {
                category.Name = categoryDto.Name;
                category.Image = categoryDto.Image;
                category.ParentCategoryId = categoryDto.ParentCategoryId;
                category.UpdatedDate = DateTime.UtcNow;
                category.UpdatedBy = updatedBy;

                // Update subcategories
                var existingSubCategories = category.SubCategories.ToList();
                var subCategories = new List<Category>();

                // Keep existing subcategories
                foreach (var subCategoryDto in categoryDto.SubCategories)
                {
                    var existingSubCategory = existingSubCategories.FirstOrDefault(sc => sc.Id == subCategoryDto.Id);
                    if (existingSubCategory != null)
                    {
                        subCategories.Add(existingSubCategory);
                    }
                }

                // Add new subcategories
                foreach (var subCategoryDto in categoryDto.SubCategories.Where(sc => sc.Id == 0))
                {
                    subCategories.Add(new Category
                    {
                        Id = subCategoryDto.Id,
                        Name = subCategoryDto.Name,
                        Image = subCategoryDto.Image,
                        IsDeleted = subCategoryDto.IsDeleted,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                        CreatedBy = updatedBy,
                        UpdatedBy = updatedBy,
                        ParentCategoryId = subCategoryDto.ParentCategoryId
                    });
                }

                category.SubCategories = subCategories;

                // Update products
                var existingProducts = category.Products.ToList();
                var products = new List<Product>();

                // Keep existing products
                foreach (var productDto in categoryDto.Products)
                {
                    var existingProduct = existingProducts.FirstOrDefault(p => p.Id == productDto.Id);
                    if (existingProduct != null)
                    {
                        products.Add(existingProduct);
                    }
                }

                // Add new products
                foreach (var productDto in categoryDto.Products.Where(p => p.Id == 0))
                {
                    products.Add(new Product
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
                        CreatedBy = updatedBy,
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
                    });
                }

                category.Products = products;

                _context.Categories.Update(category);
                _context.SaveChanges();
            }
        }

        public void SoftDeleteCategory(int id, string updatedBy)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                category.IsDeleted = true;
                category.UpdatedDate = DateTime.UtcNow;
                category.UpdatedBy = updatedBy;
                _context.Categories.Update(category);
                _context.SaveChanges();
            }
        }
    }
}