using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportsPro.Models;
using SportsPro.Models.Data;
using SportsPro.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IRepository<Customer> customerRepo;
        private readonly IRepository<Country> countryRepo;

        public CustomerController(IRepository<Customer> cRepo, IRepository<Country> coRepo)
        {
            customerRepo = cRepo;
            countryRepo = coRepo;
        }

        [Route("/customers")]
        public IActionResult List()
        {
            var customers = customerRepo.List(new QueryOptions<Customer> { OrderBy = c => c.LastName }).ToList();
            return View(customers);
        }

        [HttpGet]
        public IActionResult AddEdit(int? id)
        {
            ViewBag.Countries = GetCountriesList();

            if (id == null)
                return View(new Customer());

            var customer = customerRepo.Get(id.Value);
            return customer == null ? NotFound() : View(customer);
        }

        [HttpPost]
        public IActionResult AddEdit(Customer customer)
        {
            if (customer.CustomerID == 0 && !string.IsNullOrEmpty(customer.Email))
            {
                if (customerRepo.List(new QueryOptions<Customer> { Where = c => c.Email == customer.Email }).Any())
                {
                    ModelState.AddModelError("Email", "This email address is already in use");
                }
            }

            if (ModelState.IsValid)
            {
                if (customer.CustomerID == 0)
                    customerRepo.Insert(customer);
                else
                    customerRepo.Update(customer);

                customerRepo.Save();
                return RedirectToAction("List");
            }

            ViewBag.Countries = GetCountriesList();
            return View(customer);
        }

        private List<SelectListItem> GetCountriesList()
        {
            return countryRepo.List(new QueryOptions<Country> { OrderBy = c => c.Name })
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
            var customer = customerRepo.Get(id);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new DeleteViewModel
            {
                Id = customer.CustomerID,
                Name = customer.FirstName + " " + customer.LastName
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Delete(DeleteViewModel model)
        {
            var customer = customerRepo.Get(model.Id);
            if (customer != null)
            {
                customerRepo.Delete(customer);
                customerRepo.Save();
            }
            return RedirectToAction("List");
        }

    }
}
