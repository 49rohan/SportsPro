using Microsoft.AspNetCore.Mvc;
using SportsPro.Models.ViewModels;

namespace SportsPro.ViewComponents
{
    public class ConfirmDeleteTechnicianViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DeleteViewModel model)
        {
            return View(model);
        }
    }
}
