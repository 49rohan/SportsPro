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

        [Route("/incidents")]
        public IActionResult List()
        {
            var incidents = context.Incidents.Include(i => i.Customer).Include(i => i.Product).OrderBy(i => i.Title).ToList();
            return View(incidents);
        }

        [HttpGet]
        public IActionResult Add()
        {
            Incident incident = new Incident();
            ViewBag.Action = "Add";
            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName");
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name");
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name");
            return View("AddEdit", incident);
        }

        [HttpPost]
        public IActionResult Add(Incident incident)
        {
            if (ModelState.IsValid)
            {
                context.Incidents.Add(incident);
                context.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.Action = "Add";
            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName");
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name");
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name");
            return View("AddEdit", incident);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Incident incident = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name", incident.ProductID);
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
            ViewBag.Action = "Edit";
            return View("AddEdit", incident);
        }

        [HttpPost]
        public IActionResult Edit(Incident incident)
        {
            if (ModelState.IsValid)
            {
                context.Incidents.Update(incident);
                context.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.Customers = new SelectList(context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewBag.Products = new SelectList(context.Products, "ProductID", "Name", incident.ProductID);
            ViewBag.Technicians = new SelectList(context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
            ViewBag.Action = "Edit";
            return View("AddEdit", incident);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
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