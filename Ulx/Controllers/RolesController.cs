using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ulx.Data.Repository.IRepository;
using Ulx.Models;

namespace Ulx.Controllers
{
    public class RolesController : Controller
    {
        private static RoleManager<IdentityRole> _roleManager;
        private static UserManager<User> _userManager;
        private static IUserRepository _userRepo;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET: RolesController
        public ActionResult Index() => View(_roleManager.Roles.ToList());

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                try
                {
                    IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: RolesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RolesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RolesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RolesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                IdentityRole role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> AssignRoles(string id)
        {
            User user = _userRepo.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            AssignRolesVM model = new AssignRolesVM();
            model.AllRoles = _roleManager.Roles.ToList();
            model.UserRoles = await _userManager.GetRolesAsync(user);
            model.Email = user.Email;
            model.Id = id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> AssignRoles(string id, List<string> roles)
        {
            User user = _userRepo.Find(id);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                var result = await _userManager.AddToRolesAsync(user, addedRoles);
                result = await _userManager.RemoveFromRolesAsync(user, removedRoles);
                return RedirectToAction("Index", "Account");
            }
            return NotFound();
        }
    }
}
