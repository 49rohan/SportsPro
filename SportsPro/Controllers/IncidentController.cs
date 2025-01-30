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

        [Route("/incidents")]
        public IActionResult List()
        {
            var incidents = context.Incidents.Include(i => i.Customer).Include(i => i.Product).OrderBy(i => i.Title).ToList();
            return View(incidents);

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

            Incident incident = new Incident();
            ViewBag.Action = "Add";
            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName");
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name");
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name");
            return View("AddEdit", incident);

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

            ViewBag.Action = "Add";
            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName");
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name");
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name");
            return View("AddEdit", incident);

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            var incident = context.Incidents

            Incident incident = context.Incidents

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

            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name", incident.ProductID);
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
            ViewBag.Action = "Edit";
            return View("AddEdit", incident);

        }

        [HttpPost]
        public IActionResult Edit(IncidentEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                context.Incidents.Update(viewModel.CurrentIncident);

                context.Incidents.Update(incident);

                context.SaveChanges();
                return RedirectToAction("List");
            }


            // Reload dropdowns if invalid
            viewModel.Customers = context.Customers.ToList();
            viewModel.Products = context.Products.ToList();
            viewModel.Technicians = context.Technicians.ToList();
            viewModel.OperationType = "Edit";

            return View("AddEdit", viewModel);

            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name", incident.ProductID);
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
            ViewBag.Action = "Edit";
            return View("AddEdit", incident);

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {

            Incident incident = context.Incidents.FirstOrDefault(i => i.IncidentID == id);

            Incident incident = context.Incidents
                .FirstOrDefault(i => i.IncidentID == id);

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

}

