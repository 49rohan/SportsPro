using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.Data;
using System.Linq;

namespace SportsPro.Controllers
{
    [Authorize]
    public class TechnicianController : Controller
    {
        private readonly IRepository<Technician> techRepo;
        private readonly IRepository<Incident> incidentRepo;

        public TechnicianController(IRepository<Technician> repo, IRepository<Incident> incidentR)
        {
            techRepo = repo;
            incidentRepo = incidentR;
        }

        [Route("/technicians")]
        public IActionResult List()
        {
            var technicians = techRepo.List(new QueryOptions<Technician> { OrderBy = t => t.Name }).ToList();
            return View(technicians);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Edit", new Technician());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var technician = techRepo.Get(id);
            return technician == null ? RedirectToAction("List") : View(technician);
        }

        [HttpPost]
        public IActionResult Edit(Technician technician)
        {
            if (ModelState.IsValid)
            {
                if (technician.TechnicianID == 0)
                    techRepo.Insert(technician);
                else
                    techRepo.Update(technician);

                techRepo.Save();
                return RedirectToAction("List");
            }
            return View(technician);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var technician = techRepo.Get(id);
            return technician == null ? RedirectToAction("List") : View(technician);
        }

        [HttpPost]
        public IActionResult Delete(Technician technician)
        {
            var hasIncidents = incidentRepo.List(new QueryOptions<Incident>
            {
                Where = i => i.TechnicianID == technician.TechnicianID
            }).Any();

            if (hasIncidents)
            {
                TempData["ErrorMessage"] = "This technician cannot be deleted because they are assigned to incidents.";
                return RedirectToAction("List");
            }

            techRepo.Delete(technician);
            techRepo.Save();
            return RedirectToAction("List");
        }
    }
}
