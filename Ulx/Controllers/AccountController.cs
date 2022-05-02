using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ulx.Data.Repository.IRepository;
using Ulx.Models;

namespace Ulx.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _users;
        private readonly UserManager<User> _userManager;
        private static SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository users)
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _users = users;
        }
        public ActionResult Index() => View(_users.GetAll());

        public IActionResult LogIn()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterVM model = new RegisterVM();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    Phone = model.Phone,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, WebConstants.UserRole);
                if (result.Succeeded)
                {
                    var r = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                    
                    if (r.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginVM model = new LoginVM { RememberMe = true };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult AccessDenied(string returnUrl)
        {

            return RedirectToAction("LogIn");

        }
    }
}
