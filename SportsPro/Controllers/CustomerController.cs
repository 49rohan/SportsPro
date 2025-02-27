using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        private readonly SportsProContext _context;

        public CustomerController(SportsProContext context)
        {
            _context = context;
        }

        [Route("/customers")]
        public IActionResult List()
        {
            var customers = _context.Customers.ToList();
            return View(customers);
        }

        [HttpGet]
        public IActionResult AddEdit(int? id)
        {
            ViewBag.Countries = GetCountriesList();

            if (id == null)
            {
                return View(new Customer());
            }
            else
            {
                var customer = _context.Customers.Find(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }
        }

        [HttpPost]
        public IActionResult AddEdit(Customer customer)
        {
            // Check for unique email when adding new customer
            if (customer.CustomerID == 0 && !string.IsNullOrEmpty(customer.Email))
            {
                if (_context.Customers.Any(c => c.Email == customer.Email))
                {
                    ModelState.AddModelError("Email", "This email address is already in use");
                }
            }

            if (ModelState.IsValid)
            {
                // Rest of your existing code
            }

            ViewBag.Countries = GetCountriesList();
            return View(customer);
        }

        private List<SelectListItem> GetCountriesList()
        {
            return _context.Countries
                .Select(c => new SelectListItem
                {
                    Value = c.CountryID.ToString(),
                    Text = c.Name
                })
                .ToList();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}