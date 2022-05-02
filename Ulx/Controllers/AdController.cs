using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ulx.Data.Repository.IRepository;
using Ulx.Models;

namespace Ulx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AdController : Controller
    {
        private readonly IAdRepository _ads;
        private readonly IPictureRepository _pictures;
        private readonly IUserRepository _users;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdController(IAdRepository ads,
                            IUserRepository users,
                            IPictureRepository pictures,
                            UserManager<User> userManager,
                            RoleManager<IdentityRole> roleManager)
        {
            _ads = ads;
            _pictures= pictures;
            _userManager = userManager;
            _roleManager = roleManager;
            _users = users;
        }

        [HttpGet]
        public JsonResult Get(int id, int page, string sort)
        {

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
                ads = _ads.GetAll(filter: r=>r.CategoryId==id, includeProperties:"User");
            }
            else
            {
                ads = _ads.GetAll(includeProperties: "User");
            }
            
            int pages = (int)Math.Ceiling((decimal)ads.Count() / WebConstants.PageSize);
            if (page > pages)
                page = pages;

            switch (sort)
            {
                case "Title_Desc":
                    ads = ads.OrderByDescending(r => r.Title)
                        .Skip((page - 1) * WebConstants.PageSize)
                        .Take(WebConstants.PageSize);
                    break;
                case "Title_Asc":
                    ads = ads.OrderBy(r => r.Title)
                        .Skip((page - 1) * WebConstants.PageSize)
                        .Take(WebConstants.PageSize);
                    break;

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
            return new JsonResult(ads);
        }

        [HttpPost]
        public JsonResult Post(AdPutModel model)
        {

            if (model != null)
            {
                User user = _users.FirstOrDefault(r => r.Email == _userManager.GetUserName(User));
                Ad ad = new Ad
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    Created = model.Created,
                    UserId = (user==null ? "309248e9-908f-41a1-9722-7f9ea07115e1":user.Id)
                };
                if (_ads.Add(ad) && _ads.Save())
                {
                    foreach (var file in model.Files)
                    {
                        if (!String.IsNullOrEmpty(file))
                        {
                            Picture pic = new Picture();
                            pic.Photo = System.IO.File.ReadAllBytes(file);
                            pic.AdId = ad.Id;
                            _pictures.Add(pic);
                        }
                    }

                    _pictures.Save();
                    return new JsonResult(ad.Id);
                }
            }
            return new JsonResult("Error adding new ad");
        }

        [HttpPut]
        
        public JsonResult Put(AdPutModel model)
        {
            Ad ad = _ads.FirstOrDefault(r => r.Id == model.Id);
            if (ad != null)
            {
                ad.Title = model.Title;
                ad.Description = model.Description;
                ad.Price = model.Price;
                ad.Created = model.Created;
                ad.CategoryId = model.CategoryId;
                if (_ads.Update(ad) && _ads.Save())
                {
                    return new JsonResult("Success");
                }
            }
            return new JsonResult("Error updating an ad");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            Ad ad = _ads.FirstOrDefault(filter: r => r.Id == id);
            if (ad != null)
            {
                if (_ads.Remove(ad) && _ads.Save())
                {
                    return new JsonResult("Success");
                }
            }
            return new JsonResult("Error deleting an ad");
        }

    }
}


