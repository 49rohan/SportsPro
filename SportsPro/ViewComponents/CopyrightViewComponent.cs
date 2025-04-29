using Microsoft.AspNetCore.Mvc;
using System;

namespace SportsPro.ViewComponents
{
    public class CopyrightViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            int year = DateTime.Now.Year;
            return View("Default", year);
        }
    }
}