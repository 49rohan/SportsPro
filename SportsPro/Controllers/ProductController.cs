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

        [Route("/products")]
        public IActionResult List()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddEdit(int? id)
        {
            var product = id == null ? new Product() : _context.Products.Find(id);
            ViewBag.Action = id == null ? "Add" : "Edit";
            return View(product);
        }

        [HttpPost]
        public IActionResult AddEdit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0)
                {
                    _context.Products.Add(product);
                }
                else
                {
                    _context.Products.Update(product);
                }
                _context.SaveChanges();
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
            return RedirectToAction("List", "Product");
        }
    }
}
