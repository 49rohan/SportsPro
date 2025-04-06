using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System.Linq;

namespace SportsPro.Controllers
{
    public class RegistrationController : Controller
    {
        private SportsProContext context;

        public RegistrationController(SportsProContext ctx)
        {
            context = ctx;
        }

        // GET: Registration
        public IActionResult Index()
        {
            // Display the Get Customer page
            ViewBag.Customers = context.Customers
                .OrderBy(c => c.FirstName)
                .ToList();
            return View();
        }

        [HttpPost]
        public IActionResult List(int customerID)
        {
            if (customerID == 0)
            {
                TempData["message"] = "You must select a customer.";
                return RedirectToAction("Index");
            }
            else
            {
                var customer = context.Customers
                    .Include(c => c.Registrations)
                    .ThenInclude(r => r.Product)
                    .FirstOrDefault(c => c.CustomerID == customerID);

                // Get all products for the dropdown
                ViewBag.Products = context.Products
                    .OrderBy(p => p.Name)
                    .ToList();

                return View(customer);
            }
        }

        [HttpPost]
        public IActionResult Register(Registration registration)
        {
            // Check if the registration already exists
            var existingRegistration = context.Registrations
                .FirstOrDefault(r => r.CustomerID == registration.CustomerID &&
                                     r.ProductID == registration.ProductID);

            if (existingRegistration == null)
            {
                // Add the new registration
                context.Registrations.Add(registration);
                context.SaveChanges();
                TempData["message"] = "Product registered successfully.";
            }
            else
            {
                TempData["message"] = "This product is already registered to this customer.";
            }

            // Redirect back to the List action to show updated registrations
            return RedirectToAction("List", new { customerID = registration.CustomerID });
        }

        public IActionResult Delete(int id)
        {
            var registration = context.Registrations
                .Include(r => r.Customer)
                .Include(r => r.Product)
                .FirstOrDefault(r => r.RegistrationId == id);

            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // POST: Registration/Delete/{id}
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Find the registration
            var registration = context.Registrations.Find(id);

            if (registration != null)
            {
                // Store the customer ID before deleting the registration
                int customerID = registration.CustomerID;

                // Remove the registration
                context.Registrations.Remove(registration);
                context.SaveChanges();

                TempData["message"] = "Registration deleted successfully.";

                // Redirect to the List action with the customer ID
                return RedirectToAction("List", new { customerID = customerID });
            }

            // If registration not found, redirect to Index
            TempData["message"] = "Registration not found.";
            return RedirectToAction("Index");
        }
    }
}
