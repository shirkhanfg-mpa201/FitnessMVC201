using System.Diagnostics;
using FitnessMVC201.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessMVC201.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}
