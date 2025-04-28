using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.Data;
using SportsPro.Models.ViewModels;
using System.Linq;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product> productRepo;

        public ProductController(IRepository<Product> repo)
        {
            productRepo = repo;
        }

        [Route("/products")]
        public IActionResult List()
        {
            var products = productRepo.List(new QueryOptions<Product> { OrderBy = p => p.Name }).ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddEdit(int? id)
        {
            var product = id == null ? new Product() : productRepo.Get(id.Value);
            ViewBag.Action = id == null ? "Add" : "Edit";
            return View(product);
        }

        [HttpPost]
        public IActionResult AddEdit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0)
                    productRepo.Insert(product);
                else
                    productRepo.Update(product);

                productRepo.Save();
                TempData["message"] = $"{product.Name} was saved";
                return RedirectToAction("List");
            }

            ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = productRepo.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new DeleteViewModel
            {
                Id = product.ProductID,
                Name = product.Name
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(DeleteViewModel model)
        {
            var product = productRepo.Get(model.Id);
            if (product != null)
            {
                productRepo.Delete(product);
                productRepo.Save();
            }
            return RedirectToAction("List");
        }

    }
}
