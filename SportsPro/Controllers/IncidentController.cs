using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using SportsPro.Models.Data;
using SportsPro.Models.ViewModels;
using System.Linq;

namespace SportsPro.Controllers
{
    [Authorize]
    public class IncidentController : Controller
    {
        private readonly IRepository<Incident> incidentRepo;
        private readonly IRepository<Customer> customerRepo;
        private readonly IRepository<Product> productRepo;
        private readonly IRepository<Technician> technicianRepo;

        public IncidentController(IRepository<Incident> ir, IRepository<Customer> cr, IRepository<Product> pr, IRepository<Technician> tr)
        {
            incidentRepo = ir;
            customerRepo = cr;
            productRepo = pr;
            technicianRepo = tr;
        }

        public IActionResult List(string filter = "All")
        {
            var options = new QueryOptions<Incident>
            {
                OrderBy = i => i.Title
            };

            if (filter == "unassigned")
            {
                options.Where = i => i.TechnicianID == -1;
            }
            else if (filter == "open")
            {
                options.Where = i => i.DateClosed == null;
            }

            var incidents = incidentRepo.List(options)
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .ToList();

            var viewModel = new IncidentManagerViewModel
            {
                Incidents = incidents,
                FilterType = filter
            };

            return View(viewModel);
        }

        public IActionResult ListByTech()
        {
            var techs = technicianRepo.List(new QueryOptions<Technician> { OrderBy = t => t.Name }).ToList();
            return View(techs);
        }

        [HttpPost]
        public IActionResult SelectTechnician(int? technicianId)
        {
            if (!technicianId.HasValue)
            {
                TempData["ErrorMessage"] = "Please select a technician.";
                return RedirectToAction("ListByTech");
            }

            HttpContext.Session.SetInt32("TechnicianID", technicianId.Value);
            return RedirectToAction("IncidentsByTechnician", new { id = technicianId });
        }

        public IActionResult IncidentsByTechnician(int id)
        {
            var tech = technicianRepo.Get(id);
            if (tech == null) return RedirectToAction("ListByTech");

            var options = new QueryOptions<Incident>
            {
                Where = i => i.TechnicianID == id && i.Status == "Open",
                OrderBy = i => i.DateOpened
            };

            var incidents = incidentRepo.List(options)
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .ToList();

            var viewModel = new IncidentsByTechnicianViewModel
            {
                Technician = tech,
                Incidents = incidents
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new IncidentEditViewModel
            {
                Customers = customerRepo.List(new QueryOptions<Customer>()).ToList(),
                Products = productRepo.List(new QueryOptions<Product>()).ToList(),
                Technicians = technicianRepo.List(new QueryOptions<Technician>()).ToList(),
                CurrentIncident = new Incident(),
                OperationType = "Add"
            };

            return View("AddEdit", viewModel);
        }

        [HttpPost]
        public IActionResult Add(IncidentEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                incidentRepo.Insert(viewModel.CurrentIncident);
                incidentRepo.Save();
                return RedirectToAction("List");
            }

            viewModel.Customers = customerRepo.List(new QueryOptions<Customer>()).ToList();
            viewModel.Products = productRepo.List(new QueryOptions<Product>()).ToList();
            viewModel.Technicians = technicianRepo.List(new QueryOptions<Technician>()).ToList();
            viewModel.OperationType = "Add";

            return View("AddEdit", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var incident = incidentRepo.List(new QueryOptions<Incident>())
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            if (incident == null) return RedirectToAction("List");

            var viewModel = new IncidentEditViewModel
            {
                Customers = customerRepo.List(new QueryOptions<Customer>()).ToList(),
                Products = productRepo.List(new QueryOptions<Product>()).ToList(),
                Technicians = technicianRepo.List(new QueryOptions<Technician>()).ToList(),
                CurrentIncident = incident,
                OperationType = "Edit"
            };

            return View("AddEdit", viewModel);
        }

        [HttpPost]
        public IActionResult Edit(IncidentEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                incidentRepo.Update(viewModel.CurrentIncident);
                incidentRepo.Save();
                return RedirectToAction("List");
            }

            viewModel.Customers = customerRepo.List(new QueryOptions<Customer>()).ToList();
            viewModel.Products = productRepo.List(new QueryOptions<Product>()).ToList();
            viewModel.Technicians = technicianRepo.List(new QueryOptions<Technician>()).ToList();
            viewModel.OperationType = "Edit";

            return View("AddEdit", viewModel);
        }

        [HttpGet]
        public IActionResult EditForTechnician(int id)
        {
            var incident = incidentRepo.List(new QueryOptions<Incident>())
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            if (incident == null) return RedirectToAction("ListByTech");

            ViewBag.TechnicianID = HttpContext.Session.GetInt32("TechnicianID");
            return View("EditForTechnician", incident);
        }

        [HttpPost]
        public IActionResult EditForTechnician(Incident incident)
        {
            if (ModelState.IsValid)
            {
                incidentRepo.Update(incident);
                incidentRepo.Save();

                int? techId = HttpContext.Session.GetInt32("TechnicianID");
                return RedirectToAction("IncidentsByTechnician", new { id = techId ?? 0 });
            }

            return View("EditForTechnician", incident);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var incident = incidentRepo.Get(id);
            return incident == null ? RedirectToAction("List") : View(incident);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            incidentRepo.Delete(incident);
            incidentRepo.Save();
            return RedirectToAction("List");
        }
    }
}
