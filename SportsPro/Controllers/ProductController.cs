using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataAccess;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepo;

        public ProductController(IRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        // Displays the list of products
        public IActionResult List()
        {
            var products = _productRepo.List(new QueryOptions<Product>
            {
                OrderBy = p => p.OrderBy(prod => prod.Name)
            }).ToList();

            return View(products);
        }

        // GET: Displays the Add/Edit Product form
        [HttpGet]
        public IActionResult AddEdit(int? id)
        {
            var product = id == null ? new Product() : _productRepo.Get(id.Value);
            ViewBag.Action = id == null ? "Add" : "Edit";
            return View(product); // This will look for AddEdit.cshtml
        }

        // POST: Handles the form submission for Add/Edit
        [HttpPost]
        public IActionResult AddEdit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0) // Add new product
                {
                    _productRepo.Insert(product);
                    TempData["message"] = $"{product.Name} was added";
                }
                else // Update existing product
                {
                    _productRepo.Update(product);
                    TempData["message"] = $"{product.Name} was updated";
                }

                _productRepo.Save();
                return RedirectToAction("List");
            }

            ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _productRepo.Get(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            _productRepo.Delete(product);
            _productRepo.Save();
            TempData["delete"] = $"{product.Name} was deleted";
            return RedirectToAction("List");
        }
    }
}
