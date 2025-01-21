
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using global::SportsPro.Models;
using System.Threading.Tasks;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private readonly SportsProContext _context;

        public TechnicianController(SportsProContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var technicians = await _context.Technicians
                .Where(t => t.TechnicianID != -1)
                .OrderBy(t => t.Name)
                .ToListAsync();
            return View(technicians);
        }

        public IActionResult Add()
        {
            return View("Edit", new Technician());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var technician = _context.Technicians.Find(id);
            if (technician == null)
            {
                return NotFound();
            }
            return View(technician);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Technician technician)
        {
            if (ModelState.IsValid)
            {
                if (technician.TechnicianID == 0)
                {
                    _context.Technicians.Add(technician);
                }
                else
                {
                    _context.Technicians.Update(technician);
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(technician);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var technician = _context.Technicians.Find(id);
            if (technician == null)
            {
                return NotFound();
            }
            return View(technician);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var technician = _context.Technicians.Find(id);
            if (technician != null)
            {
                _context.Technicians.Remove(technician);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
