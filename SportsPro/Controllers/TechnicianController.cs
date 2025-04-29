using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.Data;
using SportsPro.Models.ViewModels;
using System.Linq;

namespace SportsPro.Controllers
{
    [Authorize]
    public class TechnicianController : Controller
    {
        private readonly IRepository<Technician> technicianRepo;
        private readonly IRepository<Incident> incidentRepo;

        public TechnicianController(IRepository<Technician> tRepo, IRepository<Incident> iRepo)
        {
            technicianRepo = tRepo;
            incidentRepo = iRepo;
        }

        [Route("/technicians")]
        public IActionResult List()
        {
            var technicians = technicianRepo.List(new QueryOptions<Technician> { OrderBy = t => t.Name }).ToList();
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
            var technician = technicianRepo.Get(id);
            return technician == null ? RedirectToAction("List") : View(technician);
        }

        [HttpPost]
        public IActionResult Edit(Technician technician)
        {
            if (ModelState.IsValid)
            {
                if (technician.TechnicianID == 0)
                    technicianRepo.Insert(technician);
                else
                    technicianRepo.Update(technician);

                technicianRepo.Save();
                return RedirectToAction("List");
            }
            return View(technician);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var technician = technicianRepo.Get(id);
            if (technician == null)
            {
                return NotFound();
            }

            var model = new DeleteViewModel
            {
                Id = technician.TechnicianID,
                Name = technician.Name
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(DeleteViewModel model)
        {
            var technician = technicianRepo.Get(model.Id);
            if (technician != null)
            {
                technicianRepo.Delete(technician);
                technicianRepo.Save();
            }
            return RedirectToAction("List");
        }
    }
}
