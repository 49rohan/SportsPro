using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System.ComponentModel.Design.Serialization;
using System.Linq;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private SportsProContext context;
        public IncidentController(SportsProContext context)
        {
            this.context = context;
        }

        // Gets the data from the database
        public IActionResult List() 
        {
            var incidents = context.Incidents.Include(i => i.Customer).Include(i => i.Product).OrderBy(i =>i.Title).ToList();
            return View(incidents); // Passes the list to the view to display them
        }

        [HttpGet]
        public IActionResult Add()
        {
            // Create incident object
            Incident incident = new Incident();

            ViewBag.Action = "Add";

            // Load dropdown options for Customers, Products, and Technicians
            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName");
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name");
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name");

            return View("AddEdit", incident); // Returns the page with the incident object
        }

        [HttpPost]
        public IActionResult Add(Incident incident)
        {
            // If all inputs are valid, it adds the new incident to the database
            if (ModelState.IsValid)
            {
                context.Incidents.Add(incident);
                context.SaveChanges();
                return RedirectToAction("List"); // Takes user to the list page
            }

            ViewBag.Action = "Add";

            // If inputs not valid, reloads the dropdowns and shows the form again
            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName");
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name");
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name");

            return View("AddEdit", incident); // Returns the page with the incident object
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Find the incident by ID
            Incident incident = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            // Fill in dropdown data
            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name", incident.ProductID);
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name", incident.TechnicianID);

            ViewBag.Action = "Edit";

            return View("AddEdit", incident); // Same site as the Add button, but with data filled in
        }

        [HttpPost]
        public IActionResult Edit(Incident incident)
        {
            // If all input is correct
            if (ModelState.IsValid)
            {
                // Update the incident
                context.Incidents.Update(incident);
                context.SaveChanges();
                return RedirectToAction("List"); // Takes user to the list page
            }

            // Load the dropdowns again if input is invalid
            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name", incident.ProductID);
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name", incident.TechnicianID);

            ViewBag.Action = "Edit";

            return View("AddEdit", incident); // Reload the site if input is invalid
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Loads the incident that needs to be deleted
            Incident incident = context.Incidents
                .FirstOrDefault(i => i.IncidentID == id);
            return View(incident);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            // Deletes the incident
            context.Incidents.Remove(incident);
            context.SaveChanges();
            // Takes user back to the List page
            return RedirectToAction("List");
        }
    }
}

