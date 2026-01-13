using FitnessMVC201.Contexts;
using FitnessMVC201.Models;
using FitnessMVC201.ViewModels.AccountViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessMVC201.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var isExistEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (isExistEmail != null)
            {
                ModelState.AddModelError("", "Email is already exist");
                return View(vm);
            }

            var isExistUsername = await _userManager.FindByNameAsync(vm.UserName);
            if (isExistUsername != null)
            {
                ModelState.AddModelError("", "UserName is already exist");
                return View(vm);
            }

            AppUser newUser = new AppUser()
            {
                Email = vm.Email,
                UserName = vm.UserName
            };

            var result = await _userManager.CreateAsync(newUser, vm.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            result = await _userManager.AddToRoleAsync(newUser, "Member");
            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);
                return BadRequest();
            }


            return RedirectToAction(nameof(Login));
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userManager.FindByEmailAsync(vm.Email);

            var result = await _userManager.CheckPasswordAsync(user, vm.Password);
            if (!result)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(vm);
            }

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRoles()
        {
            var adminRole = new IdentityRole()
            {
                Name = "Admin"
            };

            var memberRole = new IdentityRole()
            {
                Name = "Member"
            };

            var result = await _roleManager.CreateAsync(adminRole);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            result = await _roleManager.CreateAsync(memberRole);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok("Created");
        }
    }
}
