using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private SportsProContext _context;

        public ProductController(SportsProContext context)
        {
            _context = context;
        }

        // Displays the list of products
        public IActionResult List()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // GET: Displays the Add/Edit Product form
        [HttpGet]
        public IActionResult AddEdit(int? id)
        {
            var product = id == null ? new Product() : _context.Products.Find(id);
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
                    _context.Products.Add(product);
                }
                else // Update existing product
                {
                    _context.Products.Update(product);
                }
                _context.SaveChanges();
                TempData["message"] = $"{product.Name} was added";
                return RedirectToAction("List");
            }
            ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
            TempData["delete"] = $"{product.Name} was deleted";
            return RedirectToAction("List", "Product");
        }

        //[HttpPost]
        //public IActionResult Delete(int id)
        //{
        //    var product = _context.Products.Find(id);

        //    if (product == null)
        //    {
        //        TempData["delete"] = "Product not found!";
        //        return RedirectToAction("List", "Product");
        //    }

        //    _context.Products.Remove(product);
        //    _context.SaveChanges();

        //    TempData["delete"] = $"{product.Name} was deleted";
        //    return RedirectToAction("List", "Product");
        //}

    }
}
