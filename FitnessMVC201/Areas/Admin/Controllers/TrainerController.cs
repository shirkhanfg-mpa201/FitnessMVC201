using System.Threading.Tasks;
using FitnessMVC201.Contexts;
using FitnessMVC201.Helpers;
using FitnessMVC201.Models;
using FitnessMVC201.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace FitnessMVC201.Areas.Admin.Controllers;
[Area("Admin")]

    public class TrainerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string folderPath;

        public TrainerController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context=context;
            _environment= environment;
        folderPath = Path.Combine(_environment.WebRootPath, "images");
        }

        public async Task<IActionResult> Index()
        {

            var trainer = await _context.Trainers.Include(x=>x.Category).Select(x => new TrainerGetVm()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                CategoryName = x.Category.Name

            }).ToListAsync();
            return View(trainer);
        }


        [HttpGet]

        public async Task<IActionResult> Create()
        {
            await SendCategoriesWithViewBag();
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(TrainerCreateVm vm)
        {

            await SendCategoriesWithViewBag();
            if (!ModelState.IsValid) return View(vm);

            var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);

            if (!isExistCategory)
            {
                ModelState.AddModelError("CategoryId", "This category not found");
                return View(vm);
            }


            if (vm.ImageUrl.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageUrl", "It must be max 2 mb");
                return View(vm);

            }


            if (!vm.ImageUrl.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageUrl", "It must be image type");
                return View(vm);
            }


            string uniqueFileName = Guid.NewGuid().ToString() + folderPath;

            string path = Path.Combine(folderPath, uniqueFileName);


            Trainer trainer = new Trainer()
            {
                Name = vm.Name,
                Description = vm.Description,
                ImageUrl = uniqueFileName,
                CategoryId = vm.CategoryId

            };

            await _context.Trainers.AddAsync(trainer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }




        public async Task<IActionResult> Delete(int id)
        {

            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer == null) return NotFound();


            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();


            var deletedPath = Path.Combine(folderPath, trainer.ImageUrl);

            if (System.IO.File.Exists(deletedPath)) System.IO.File.Delete(deletedPath);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {

            var trainer = await _context.Trainers.FindAsync(id);

            TrainerUpdateVm vm = new TrainerUpdateVm()
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Description = trainer.Description,
                CategoryId = trainer.CategoryId


            };
            await SendCategoriesWithViewBag();
            return View(vm);


        }


        [HttpPost]
        public async Task<IActionResult> Update(TrainerUpdateVm vm)
        {


            await SendCategoriesWithViewBag();

            if (!ModelState.IsValid) { return View(vm); }

            var trainer = await _context.Trainers.FindAsync(vm.Id);
            if (trainer == null) { return NotFound(); }
            var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!isExistCategory)
            {
                ModelState.AddModelError("CategoryId", "This category not found");
                return View(vm);
            }

            if (vm.ImageUrl?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("ImageUrl", "It must be max 2 mb");
                return View(vm);
            }
            if (vm.ImageUrl?.CheckType("image") ?? false)
            {
                ModelState.AddModelError("ImageUrl", "It must be image type");
                return View(vm);
            }


            trainer.Name = vm.Name;
            trainer.Description = vm.Description;
            trainer.CategoryId = vm.CategoryId;
            trainer.Description = vm.Description;

            if (vm.ImageUrl is { }) {
                string newImagePath =await vm.ImageUrl.FileUploadAsync(folderPath);

                string deletedImagePath = Path.Combine(folderPath,trainer.ImageUrl);

                trainer.ImageUrl = newImagePath;
            }
            _context.Trainers.Update(trainer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task SendCategoriesWithViewBag()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
        }
    }

