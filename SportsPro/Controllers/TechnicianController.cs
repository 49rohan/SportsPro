﻿using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using System.Linq;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private SportsProContext context;

        public TechnicianController(SportsProContext ctx)
        {
            context = ctx;
        }

        [Route("/technicians")]
        public IActionResult List()
        {
            var technicians = context.Technicians.OrderBy(t => t.Name).ToList();
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
            var technician = context.Technicians.Find(id);
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
                    context.Technicians.Add(technician);
                else
                    context.Technicians.Update(technician);

                context.SaveChanges();
                return RedirectToAction("List"); 
            }
            return View(technician);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var technician = context.Technicians.Find(id);
            if (technician == null)
            {
                return RedirectToAction("List"); 
            }
            return View(technician);
        }

        [HttpPost]
        public IActionResult Delete(Technician technician)
        {
            bool hasIncidents = context.Incidents.Any(i => i.TechnicianID == technician.TechnicianID);

            if (hasIncidents)
            {
                TempData["ErrorMessage"] = "This technician cannot be deleted because they are assigned to incidents.";
                return RedirectToAction("List");
            }

            context.Technicians.Remove(technician);
            context.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
