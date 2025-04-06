using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportsPro.Models;
using SportsPro.Models.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<Country> _countryRepo;

        public CustomerController(IRepository<Customer> customerRepo, IRepository<Country> countryRepo)
        {
            _customerRepo = customerRepo;
            _countryRepo = countryRepo;
        }

       
        public IActionResult List()
        {
            var customers = _customerRepo.List(new QueryOptions<Customer>
            {
                OrderBy = c => c.OrderBy(cust => cust.LastName)
            });
            return View(customers);
        }

        [HttpGet]
        public IActionResult AddEdit(int? id) // Use nullable int for id
        {
            ViewBag.Countries = GetCountriesList();

            if (id == null)
            {
                // Add operation
                return View(new Customer());
            }
            else
            {
                var customer = _customerRepo.Get(id.Value);
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
            if (customer.CustomerID == 0 && !string.IsNullOrEmpty(customer.Email))
            {
                var exists = _customerRepo.List(new QueryOptions<Customer>
                {
                    Where = c => c.Email == customer.Email
                }).Any();

                if (exists)
                {
                    _context.Customers.Add(customer);
                }
                else // Edit operation
                {
                    _context.Attach(customer);
                    _context.Entry(customer).State = EntityState.Modified;
                }

            if (ModelState.IsValid)
            {
                if (customer.CustomerID == 0)
                {
                    _customerRepo.Insert(customer);
                }
                else
                {
                    _customerRepo.Update(customer);
                }

                _customerRepo.Save();
                return RedirectToAction("List");
            }

            ViewBag.Countries = GetCountriesList();
            return View(customer);
        }


        private List<SelectListItem> GetCountriesList()
        {
            return _countryRepo.List(new QueryOptions<Country>
            {
                OrderBy = c => c.OrderBy(c => c.Name)
            })
            .Select(c => new SelectListItem
            {
                Value = c.CountryID,
                Text = c.Name
            })
            .ToList();
        }


       
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = _customerRepo.Get(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }


        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _customerRepo.Get(id);
            if (customer != null)
            {
                _customerRepo.Delete(customer);
                _customerRepo.Save();
            }
            return RedirectToAction("List");
        }


    }
}
