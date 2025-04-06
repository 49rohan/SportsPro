using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataAccess;
using System.Linq;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private readonly IRepository<Technician> _technicianRepo;
        private readonly IRepository<Incident> _incidentRepo;

        public TechnicianController(IRepository<Technician> technicianRepo, IRepository<Incident> incidentRepo)
        {
            _technicianRepo = technicianRepo;
            _incidentRepo = incidentRepo;
        }

        public IActionResult List()
        {
            var technicians = _technicianRepo.List(new QueryOptions<Technician>
            {
                OrderBy = t => t.OrderBy(x => x.Name)
            }).ToList();

            return View(technicians);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var technician = new Technician();
            return View("Edit", technician);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var technician = _technicianRepo.Get(id);
            if (technician == null)
            {
                return RedirectToAction("List");
            }
            return View(technician);
        }

        [HttpPost]
        public IActionResult Edit(Technician technician)
        {
            if (ModelState.IsValid)
            {
                if (technician.TechnicianID == 0)
                    _technicianRepo.Insert(technician);
                else
                    _technicianRepo.Update(technician);

                _technicianRepo.Save();
                return RedirectToAction("List");
            }

            return View(technician);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var technician = _technicianRepo.Get(id);
            if (technician == null)
            {
                return RedirectToAction("List");
            }
            return View(technician);
        }

        [HttpPost]
        public IActionResult Delete(Technician technician)
        {
            bool hasIncidents = _incidentRepo.List(new QueryOptions<Incident>
            {
                Where = i => i.TechnicianID == technician.TechnicianID
            }).Any();

            if (hasIncidents)
            {
                TempData["ErrorMessage"] = "This technician cannot be deleted because they are assigned to incidents.";
                return RedirectToAction("List");
            }

            _technicianRepo.Delete(technician);
            _technicianRepo.Save();
            return RedirectToAction("List");
        }
    }
}