using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Ulx.Data.Repository.IRepository;
using Ulx.Models;

namespace Ulx.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static UserManager<User> _userManager;
        private readonly IUserRepository _users;
        private readonly IPictureRepository _pictures;
        private readonly IAdRepository _ads;
        private readonly ICategoryRepository _categories;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger,
                    IUserRepository users,
                    ICategoryRepository categories,
                    IAdRepository ads,
                    IPictureRepository pictures,
                    RoleManager<IdentityRole> roleManager,
                    UserManager<User> userManager,
                    IWebHostEnvironment webHostEnvironment)

        {
            _logger = logger;
            _users = users;
            _ads = ads;
            _categories = categories;
            _pictures = pictures;
            _roleManager = roleManager;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(int id, int page, string sort)
        {
            await CreateAdminAndRoles();

            if (page == null || page == 0)
            {
                page = 1;
            }

            if (String.IsNullOrEmpty(sort))
            {
                sort = "Created_Desc";
            }



            IEnumerable<Ad> ads = new List<Ad>();
            if (id != 0)
            {
                ads = _ads.GetAll(filter: r=>r.CategoryId==id);
            }
            else
            {
                ads = _ads.GetAll();
            }
            
            int pages = (int)Math.Ceiling((decimal)ads.Count() / WebConstants.PageSize);
            if (page > pages)
                page = pages;

            switch (sort)
            {
                case "Created_Desc":
                    ads = ads.OrderByDescending(r => r.Created)
                        .Skip((page - 1) * WebConstants.PageSize)
                        .Take(WebConstants.PageSize);
                    break;
                case "Created_Asc":
                    ads = ads.OrderBy(r => r.Created)
                        .Skip((page - 1) * WebConstants.PageSize)
                        .Take(WebConstants.PageSize);
                    break;
                case "Price_Desc":
                    ads = ads.OrderByDescending(r => r.Price)
                        .Skip((page - 1) * WebConstants.PageSize)
                        .Take(WebConstants.PageSize);
                    break;
                case "Price_Asc":
                    ads = ads.OrderBy(r => r.Price)
                        .Skip((page - 1) * WebConstants.PageSize)
                        .Take(WebConstants.PageSize);
                    break;

            }

            ViewBag.Category = id;
            ViewBag.SortField = sort;
            ViewBag.CurrentPage = page;
            ViewBag.Pages = pages;
            return View(ads);
        }

        public IActionResult Categories()
        {
            return View("Categories");
        }

        public IActionResult Ads()
        {
            return View("Ads");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult ShowAd(int id)
        {

            return View();
        }
        
        [Authorize(Roles = "Admin,User")]
        public IActionResult NewAdd()
        {
            ViewBag.Categories = _categories.GetAll();
            AdViewModel model = new AdViewModel
            {
                Id = 0,
                CategoryId = _categories.FirstOrDefault().Id
            };
            return View("EditAdd", model);
        }

        public IActionResult EditAd(int id)
        {
            ViewBag.Categories = _categories.GetAll();
            AdViewModel model = new AdViewModel();
            Ad ad = _ads.FirstOrDefault(r => r.Id == id);
            if (ad != null)
            {
                model.Id = ad.Id;
                model.CategoryId = ad.CategoryId;
                model.Title = ad.Title;
                model.Description = ad.Description;
                model.Price = ad.Price;
            }

            return View("EditAdd", model);
        }

        public FileContentResult GetPhoto(int id, int count)
        {
            List<Picture> pictures = _pictures.GetAll(r => r.AdId == id).ToList();
            {
                if (pictures.Count != 0 && count <=pictures.Count-1)
                {
                    Picture pic = pictures[count];
                    if (pic != null)
                    {
                        return File(pic.Photo, "image/jpeg");
                    }
                }
            }
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", "NoPhoto.jpg");
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return new FileContentResult(bytes, "image/jpeg");
        }

        public IActionResult SaveAd(AdViewModel model)
        {
        
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (model.Id == 0) //Add new ad
                {
                    User user = _users.FirstOrDefault(r => r.Email == _userManager.GetUserName(User));
                    Ad ad = new Ad
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Price = model.Price,
                        CategoryId = model.CategoryId,
                        Created = DateTime.Now,
                        UserId = user.Id
                    };
                    if (_ads.Add(ad) && _ads.Save())
                    {
                        foreach (var file in files)
                        {
                            if (!String.IsNullOrEmpty(file.FileName))
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    file.CopyTo(memoryStream);
                                    Picture pic = new Picture
                                    {
                                        Photo = memoryStream.ToArray(),
                                        AdId = ad.Id
                                    };
                                    _pictures.Add(pic);
                                }
                            }
                        }
                        _pictures.Save();
                        return RedirectToAction("Ads", "Home");
                    }
                }
                else //Edit an ad
                {
                    Ad ad = _ads.FirstOrDefault(r => r.Id == model.Id);
                    if (ad != null)
                    {
                        ad.Title = model.Title;
                        ad.Description = model.Description;
                        ad.Price = model.Price;
                        ad.CategoryId = model.CategoryId;
                        if (_ads.Update(ad) && _ads.Save())
                        {
                            IEnumerable<Picture> pictures = _pictures.GetAll(filter: r => r.AdId == ad.Id);
                            foreach (var pic in pictures)
                            {
                                _pictures.Remove(pic);
                            }
                            foreach (var file in files)
                            {
                                if (!String.IsNullOrEmpty(file.FileName))
                                {
                                    using (var memoryStream = new MemoryStream())
                                    {
                                        file.CopyTo(memoryStream);
                                        Picture pic = new Picture
                                        {
                                            Photo = memoryStream.ToArray(),
                                            AdId = ad.Id
                                        };
                                        _pictures.Add(pic);
                                    }
                                }
                            }
                            _pictures.Save();
                            return RedirectToAction("Ads", "Home");
                        }
                    }
                }
            }
            return RedirectToAction("NewAdd");

        }

        [HttpGet]
        public JsonResult GetAd(int id)
        {

            Ad ad = _ads.FirstOrDefault(r => r.Id == id,includeProperties:"User");
            if (ad == null)
            {
                return new JsonResult(new AdDtoModel());
            }

            string host = HttpContext.Request.Host.Value;
            AdDtoModel model = new AdDtoModel
            {
                Id=ad.Id,
                Title = ad.Title,
                Description = ad.Description,
                Price = ad.Price,
                Created = ad.Created,
                CategoryId = ad.CategoryId,
                Name = ad.User.Name,
                Email=ad.User.Email,
                Phone = ad.User.Phone,
                Files = new List<string>()
            };
            IEnumerable<Picture> pictures = _pictures.GetAll(filter: r => r.AdId == id);

            int i = 0;
            foreach (var pic in pictures)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", "tmp",id.ToString()+"_"+i.ToString() + ".jpg");
                path=path.Replace('\\', '/');              
                using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
                {
                     fstream.Write(pic.Photo, 0, pic.Photo.Length);
                }
                path = Path.Combine("images", "tmp",id.ToString()+"_"+i++.ToString() + ".jpg");
                path=path.Replace('\\', '/');              
                model.Files.Add(path);
            }
            return new JsonResult(model);
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task CreateAdminAndRoles()
        {
            IdentityRole role = await _roleManager.FindByNameAsync(WebConstants.AdminRole);
            if (role == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(WebConstants.AdminRole));
            }

            role = await _roleManager.FindByNameAsync(WebConstants.UserRole);
            if (role == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(WebConstants.UserRole));
            }

            User admin = await _userManager.FindByEmailAsync(WebConstants.AdminEmail);
            if (admin == null)
            {
                admin = new User
                {
                    Email = WebConstants.AdminEmail,
                    UserName = WebConstants.AdminEmail,
                    Name = "Administrator",
                    Phone = "555-55-55"
                };
                var res = await _userManager.CreateAsync(admin, WebConstants.AdminPassword);
                res = await _userManager.AddToRoleAsync(admin, WebConstants.AdminRole);
            }
        }
    }
}