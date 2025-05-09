using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.DTOs;
using WebApplicationTest.Services;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WebApplicationTest.Models;

namespace WebApplicationTest.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ICategoryService categoryService, UserManager<ApplicationUser> userManager)
        {
            _categoryService = categoryService;
            _userManager = userManager;
        }

        // GET: /Category/Index
        public IActionResult Index()
        {
            var categories = _categoryService.GetAllCategories();
            return View(categories);
        }

        // GET: /Category/Details/{id}
        public IActionResult Details(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: /Category/Create
        public IActionResult Create()
        {
            var parentCategories = _categoryService.GetAllCategories().Where(c => c.ParentCategoryId == null).ToList();
            ViewBag.ParentCategories = parentCategories;
            return View();
        }

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryDto categoryDto, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Save the image
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories", ImageFile.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }
                    categoryDto.Image = "/images/categories/" + ImageFile.FileName;
                }

                var userName = GetCurrentUserName();
                _categoryService.AddCategory(categoryDto, userName, userName);
                return RedirectToAction("Index");
            }

            var parentCategories = _categoryService.GetAllCategories().Where(c => c.ParentCategoryId == null).ToList();
            ViewBag.ParentCategories = parentCategories;
            return View(categoryDto);
        }

        // GET: /Category/Edit/{id}
        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            var parentCategories = _categoryService.GetAllCategories().Where(c => c.ParentCategoryId == null).ToList();
            ViewBag.ParentCategories = parentCategories;

            return View(category);
        }

        // POST: /Category/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryDto categoryDto, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Update the image if a new one is uploaded
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories", ImageFile.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }
                    categoryDto.Image = "/images/categories/" + ImageFile.FileName;
                }

                var userName = GetCurrentUserName();
                _categoryService.UpdateCategory(categoryDto, userName);
                return RedirectToAction("Index");
            }

            var parentCategories = _categoryService.GetAllCategories().Where(c => c.ParentCategoryId == null).ToList();
            ViewBag.ParentCategories = parentCategories;

            return View(categoryDto);
        }

        // GET: /Category/Delete/{id}
        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: /Category/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userName = GetCurrentUserName();
            _categoryService.SoftDeleteCategory(id, userName);
            return RedirectToAction("Index");
        }

        private string GetCurrentUserName()
        {
            return _userManager.GetUserName(User);
        }
    }
}