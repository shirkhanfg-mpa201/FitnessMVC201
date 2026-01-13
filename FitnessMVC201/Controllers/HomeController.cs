using FitnessMVC201.Contexts;
using FitnessMVC201.Models;
using FitnessMVC201.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FitnessMVC201.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var trainer = await _context.Trainers.Select(x => new TrainerGetVm()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                CategoryName = x.Category.Name

            }).ToListAsync();
            return View(trainer);
        }

    }
}
