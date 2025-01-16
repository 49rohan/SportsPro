using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
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

        public IActionResult List() 
        {
            var incidents = context.Incidents.Include(i => i.Customer).Include(i => i.Product).OrderBy(i =>i.Title).ToList();
            return View(incidents);
        }

    }
}
