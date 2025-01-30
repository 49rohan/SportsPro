using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using SportsPro.Models.ViewModels;
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

        // Uses IncidentManagerViewModel instead of directly returning incidents
        public IActionResult List(string filter = "All")
        {
            var incidents = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .OrderBy(i => i.Title)
                .ToList();

            var viewModel = new IncidentManagerViewModel
            {
                Incidents = incidents,
                FilterType = filter
            };

            return View(viewModel); // Passes the view model instead of the raw list
        }

        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new IncidentEditViewModel
            {
                Customers = context.Customers.ToList(),
                Products = context.Products.ToList(),
                Technicians = context.Technicians.ToList(),
                CurrentIncident = new Incident(),
                OperationType = "Add"
            };

            return View("AddEdit", viewModel); // Uses ViewModel
        }

        [HttpPost]
        public IActionResult Add(IncidentEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                context.Incidents.Add(viewModel.CurrentIncident);
                context.SaveChanges();
                return RedirectToAction("List");
            }

            // Reload dropdowns if invalid
            viewModel.Customers = context.Customers.ToList();
            viewModel.Products = context.Products.ToList();
            viewModel.Technicians = context.Technicians.ToList();
            viewModel.OperationType = "Add";

            return View("AddEdit", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var incident = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            var viewModel = new IncidentEditViewModel
            {
                Customers = context.Customers.ToList(),
                Products = context.Products.ToList(),
                Technicians = context.Technicians.ToList(),
                CurrentIncident = incident,
                OperationType = "Edit"
            };

            return View("AddEdit", viewModel); // Uses ViewModel
        }

        [HttpPost]
        public IActionResult Edit(IncidentEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                context.Incidents.Update(viewModel.CurrentIncident);
                context.SaveChanges();
                return RedirectToAction("List");
            }

            // Reload dropdowns if invalid
            viewModel.Customers = context.Customers.ToList();
            viewModel.Products = context.Products.ToList();
            viewModel.Technicians = context.Technicians.ToList();
            viewModel.OperationType = "Edit";

            return View("AddEdit", viewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Incident incident = context.Incidents.FirstOrDefault(i => i.IncidentID == id);
            return View(incident);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            context.Incidents.Remove(incident);
            context.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
