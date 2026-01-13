using Microsoft.AspNetCore.Mvc;

namespace FitnessMVC201.Areas.Admin.Controllers;
    [Area("Admin")]

    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

