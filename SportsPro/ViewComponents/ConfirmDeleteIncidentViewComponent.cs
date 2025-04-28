using Microsoft.AspNetCore.Mvc;
using SportsPro.Models.ViewModels;

namespace SportsPro.ViewComponents
{
    public class ConfirmDeleteIncidentViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DeleteViewModel model)
        {
            return View(model);
        }
    }
}
