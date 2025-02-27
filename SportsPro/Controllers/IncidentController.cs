using Microsoft.AspNetCore.Http;
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
        private readonly SportsProContext context;

        public IncidentController(SportsProContext context)
        {
            this.context = context;
        }

        //  List all incidents (Incident Manager View)
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

            return View(viewModel);
        }

        //  Technician selection page
        public IActionResult ListByTech()
        {
            var technicians = context.Technicians.OrderBy(t => t.Name).ToList();
            return View(technicians);
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

        //  List Open Incidents for a Technician
        public IActionResult IncidentsByTechnician(int id)
        {
            var technician = context.Technicians.Find(id);
            if (technician == null)
            {
                return RedirectToAction("ListByTech");
            }

            var incidents = context.Incidents
                .Where(i => i.TechnicianID == id && i.Status == "Open")
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .OrderBy(i => i.DateOpened)
                .ToList();

            var viewModel = new IncidentsByTechnicianViewModel
            {
                Technician = technician,
                Incidents = incidents
            };

            return View(viewModel);
        }

        //  Add Incident (Incident Manager)
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

            return View("AddEdit", viewModel);
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

            viewModel.Customers = context.Customers.ToList();
            viewModel.Products = context.Products.ToList();
            viewModel.Technicians = context.Technicians.ToList();
            viewModel.OperationType = "Add";

            return View("AddEdit", viewModel);
        }

        //  Edit Incident (Incident Manager)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var incident = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            if (incident == null)
            {
                return RedirectToAction("List");
            }

            var viewModel = new IncidentEditViewModel
            {
                Customers = context.Customers.ToList(),
                Products = context.Products.ToList(),
                Technicians = context.Technicians.ToList(),
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
                context.Incidents.Update(viewModel.CurrentIncident);
                context.SaveChanges();
                return RedirectToAction("List");
            }

            viewModel.Customers = context.Customers.ToList();
            viewModel.Products = context.Products.ToList();
            viewModel.Technicians = context.Technicians.ToList();
            viewModel.OperationType = "Edit";

            return View("AddEdit", viewModel);
        }

        //  Edit Incident (Technician)
        [HttpGet]
        public IActionResult EditForTechnician(int id)
        {
            var incident = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            if (incident == null)
            {
                return RedirectToAction("ListByTech");
            }

            //  Retrieve TechnicianID from session for redirecting back
            int? technicianID = HttpContext.Session.GetInt32("TechnicianID");

            ViewBag.TechnicianID = technicianID;
            return View("EditForTechnician", incident);
        }

        [HttpPost]
        public IActionResult EditForTechnician(Incident incident)
        {
            if (ModelState.IsValid)
            {
                context.Incidents.Update(incident);
                context.SaveChanges();

                //  Retrieve TechnicianID from session for proper redirect
                int? technicianID = HttpContext.Session.GetInt32("TechnicianID");

                if (technicianID == null || technicianID == 0)
                {
                    return RedirectToAction("ListByTech"); // Fallback if Technician ID is missing
                }

                return RedirectToAction("IncidentsByTechnician", new { id = technicianID });
            }

            return View("EditForTechnician", incident);
        }



        //  Delete Incident
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var incident = context.Incidents.FirstOrDefault(i => i.IncidentID == id);
            if (incident == null)
            {
                return RedirectToAction("List");
            }
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
