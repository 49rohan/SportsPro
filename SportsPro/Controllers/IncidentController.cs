using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using SportsPro.Models.DataAccess;
using SportsPro.Models.ViewModels;
using System.Linq;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private readonly IRepository<Incident> _incidentRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<Technician> _technicianRepo;

        public IncidentController(
            IRepository<Incident> incidentRepo,
            IRepository<Customer> customerRepo,
            IRepository<Product> productRepo,
            IRepository<Technician> technicianRepo)
        {
            _incidentRepo = incidentRepo;
            _customerRepo = customerRepo;
            _productRepo = productRepo;
            _technicianRepo = technicianRepo;
        }

        public IActionResult List(string filter = "All")
        {
            var query = new QueryOptions<Incident>
            {
                OrderBy = i => i.OrderBy(x => x.Title),
            };

            var incidents = _incidentRepo.List(query)
                .ToList();

            if (filter == "unassigned")
            {
                incidents = incidents.Where(i => i.TechnicianID == -1).ToList();
            }
            else if (filter == "open")
            {
                incidents = incidents.Where(i => i.DateClosed == null).ToList();
            }

            var viewModel = new IncidentManagerViewModel
            {
                Incidents = incidents,
                FilterType = filter
            };

            return View(viewModel);
        }

        public IActionResult ListByTech()
        {
            var technicians = _technicianRepo.List(new QueryOptions<Technician>
            {
                OrderBy = t => t.OrderBy(x => x.Name)
            }).ToList();

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

        public IActionResult IncidentsByTechnician(int id)
        {
            var technician = _technicianRepo.Get(id);
            if (technician == null)
            {
                return RedirectToAction("ListByTech");
            }

            var incidents = _incidentRepo.List(new QueryOptions<Incident>
            {
                Where = i => i.TechnicianID == id && i.Status == "Open",
                OrderBy = i => i.OrderBy(x => x.DateOpened)
            }).ToList();

            var viewModel = new IncidentsByTechnicianViewModel
            {
                Technician = technician,
                Incidents = incidents
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new IncidentEditViewModel
            {
                Customers = _customerRepo.List(new QueryOptions<Customer>()).ToList(),
                Products = _productRepo.List(new QueryOptions<Product>()).ToList(),
                Technicians = _technicianRepo.List(new QueryOptions<Technician>()).ToList(),
                CurrentIncident = new Incident(),
                OperationType = "Add"
            };

            return View("AddEdit", viewModel);
        }

        [HttpPost]
        public IActionResult Add(Incident incident)
        {
            // If all inputs are valid, it adds the new incident to the database
            if (ModelState.IsValid)
            {
                _incidentRepo.Insert(viewModel.CurrentIncident);
                _incidentRepo.Save();
                return RedirectToAction("List");
            }

            viewModel.Customers = _customerRepo.List(new QueryOptions<Customer>()).ToList();
            viewModel.Products = _productRepo.List(new QueryOptions<Product>()).ToList();
            viewModel.Technicians = _technicianRepo.List(new QueryOptions<Technician>()).ToList();
            viewModel.OperationType = "Add";

            return View("AddEdit", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var incident = _incidentRepo.List(new QueryOptions<Incident>
            {
                Where = i => i.IncidentID == id
            }).FirstOrDefault();

            if (incident == null)
            {
                return RedirectToAction("List");
            }

            var viewModel = new IncidentEditViewModel
            {
                Customers = _customerRepo.List(new QueryOptions<Customer>()).ToList(),
                Products = _productRepo.List(new QueryOptions<Product>()).ToList(),
                Technicians = _technicianRepo.List(new QueryOptions<Technician>()).ToList(),
                CurrentIncident = incident,
                OperationType = "Edit"
            };

            return View("AddEdit", viewModel);
        }

        [HttpPost]
        public IActionResult Edit(Incident incident)
        {
            // If all input is correct
            if (ModelState.IsValid)
            {
                _incidentRepo.Update(viewModel.CurrentIncident);
                _incidentRepo.Save();
                return RedirectToAction("List");
            }

            viewModel.Customers = _customerRepo.List(new QueryOptions<Customer>()).ToList();
            viewModel.Products = _productRepo.List(new QueryOptions<Product>()).ToList();
            viewModel.Technicians = _technicianRepo.List(new QueryOptions<Technician>()).ToList();
            viewModel.OperationType = "Edit";

            return View("AddEdit", viewModel);
        }

        [HttpGet]
        public IActionResult EditForTechnician(int id)
        {
            var incident = _incidentRepo.Get(id);
            if (incident == null)
            {
                return RedirectToAction("ListByTech");
            }

            int? technicianID = HttpContext.Session.GetInt32("TechnicianID");
            ViewBag.TechnicianID = technicianID;

            return View("EditForTechnician", incident);
        }

        [HttpPost]
        public IActionResult EditForTechnician(Incident incident)
        {
            if (ModelState.IsValid)
            {
                _incidentRepo.Update(incident);
                _incidentRepo.Save();

                int? technicianID = HttpContext.Session.GetInt32("TechnicianID");

                if (technicianID == null || technicianID == 0)
                {
                    return RedirectToAction("ListByTech");
                }

                return RedirectToAction("IncidentsByTechnician", new { id = technicianID });
            }

            return View("AddEdit", incident); // Reload the site if input is invalid
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var incident = _incidentRepo.Get(id);
            if (incident == null)
            {
                return RedirectToAction("List");
            }
            return View(incident);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var incident = _incidentRepo.Get(id);
            if (incident != null)
            {
                _incidentRepo.Delete(incident);
                _incidentRepo.Save();
            }
            return RedirectToAction("List");
        }
    }
}