using FitnessMVC201.Contexts;
using FitnessMVC201.Models;
using FitnessMVC201.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessMVC201.Areas.Admin.Controllers;
    [Area("Admin")]

    public class CategoryController(AppDbContext _context) : Controller
    {
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.Select(x => new CategoryGetVM()
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Category newCategory = new Category()
        {
            Name = vm.Name
        };

        await _context.AddAsync(newCategory);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var vm = new CategoryUpdateVM()
        {
            Id = category.Id,
            Name = category.Name
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CategoryUpdateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var existCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == vm.Id);
        if (existCategory == null)
        {
            return NotFound();
        }

        existCategory.Name = vm.Name;

        _context.Categories.Update(existCategory);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}

