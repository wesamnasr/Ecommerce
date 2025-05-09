using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.DTOs;
using WebApplicationTest.Services;
using WebApplicationTest.ViewModels;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WebApplicationTest.Models;

namespace WebApplicationTest.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        // GET: /Product/Index
        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        // GET: /Product/Details/{id}
        public IActionResult Details(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            var categories = _categoryService.GetAllCategories().Where(c => c.ParentCategoryId == null).ToList();
            ViewBag.Categories = categories;
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel viewModel, IFormFile MainImageFile, IEnumerable<IFormFile> AdditionalImagesFiles)
        {
            if (ModelState.IsValid)
            {
                var productDto = viewModel.Product;

                // Save the main image
                if (MainImageFile != null && MainImageFile.Length > 0)
                {
                    var mainImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", MainImageFile.FileName);
                    using (var stream = new FileStream(mainImagePath, FileMode.Create))
                    {
                        MainImageFile.CopyTo(stream);
                    }
                    productDto.MainImageUrl = "/images/products/" + MainImageFile.FileName;
                }

                // Save additional images
                var productImages = new List<ProductImageDto>();
                if (AdditionalImagesFiles != null && AdditionalImagesFiles.Count() > 0)
                {
                    foreach (var imageFile in AdditionalImagesFiles)
                    {
                        if (imageFile.Length > 0)
                        {
                            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", imageFile.FileName);
                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                imageFile.CopyTo(stream);
                            }

                            productImages.Add(new ProductImageDto
                            {
                                ImageUrl = "/images/products/" + imageFile.FileName,
                                IsMainImage = false,  // Set to false by default
                                IsDeleted = false,  // Set to false by default
                                CreatedBy = GetCurrentUserName(),
                                UpdatedBy = GetCurrentUserName()
                            });
                        }
                    }
                }
                productDto.Images = productImages;

                var userName = GetCurrentUserName();
                _productService.AddProduct(productDto, userName, userName);
                return RedirectToAction("Index");
            }

            var categories = _categoryService.GetAllCategories().Where(c => c.ParentCategoryId == null).ToList();
            ViewBag.Categories = categories;
            return View(viewModel);
        }

        // GET: /Product/Edit/{id}
        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductViewModel
            {
                Product = product,
                MainImageUrl = product.MainImageUrl,
                AdditionalImageUrls = product.Images.Where(i => !i.IsDeleted).Select(i => i.ImageUrl).ToList()
            };

            var categories = _categoryService.GetAllCategories().Where(c => c.ParentCategoryId == null).ToList();
            ViewBag.Categories = categories;

            return View(viewModel);
        }

        // POST: /Product/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel viewModel, IFormFile MainImageFile, IEnumerable<IFormFile> AdditionalImagesFiles)
        {
            if (ModelState.IsValid)
            {
                var productDto = viewModel.Product;

                // Update the main image if a new one is uploaded
                if (MainImageFile != null && MainImageFile.Length > 0)
                {
                    var mainImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", MainImageFile.FileName);
                    using (var stream = new FileStream(mainImagePath, FileMode.Create))
                    {
                        MainImageFile.CopyTo(stream);
                    }
                    productDto.MainImageUrl = "/images/products/" + MainImageFile.FileName;
                }

                // Update additional images
                var existingImages = productDto.Images.ToList();
                var productImages = new List<ProductImageDto>();

                // Keep existing images
                foreach (var imageUrl in viewModel.AdditionalImageUrls)
                {
                    var existingImage = existingImages.FirstOrDefault(i => i.ImageUrl == imageUrl);
                    if (existingImage != null)
                    {
                        productImages.Add(existingImage);
                    }
                }

                // Add new images
                if (AdditionalImagesFiles != null && AdditionalImagesFiles.Count() > 0)
                {
                    foreach (var imageFile in AdditionalImagesFiles)
                    {
                        if (imageFile.Length > 0)
                        {
                            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", imageFile.FileName);
                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                imageFile.CopyTo(stream);
                            }

                            productImages.Add(new ProductImageDto
                            {
                                ImageUrl = "/images/products/" + imageFile.FileName,
                                IsMainImage = false,  // Set to false by default
                                IsDeleted = false,  // Set to false by default
                                CreatedBy = GetCurrentUserName(),
                                UpdatedBy = GetCurrentUserName()
                            });
                        }
                    }
                }

                productDto.Images = productImages;

                var userName = GetCurrentUserName();
                _productService.UpdateProduct(productDto, userName);
                return RedirectToAction("Index");
            }

            var categories = _categoryService.GetAllCategories().Where(c => c.ParentCategoryId == null).ToList();
            ViewBag.Categories = categories;

            return View(viewModel);
        }

        // GET: /Product/Delete/{id}
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: /Product/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userName = GetCurrentUserName();
            _productService.SoftDeleteProduct(id, userName);
            return RedirectToAction("Index");
        }

        private string GetCurrentUserName()
        {
            return _userManager.GetUserName(User);
        }
    }
}