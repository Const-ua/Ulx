using Microsoft.AspNetCore.Mvc;
using Ulx.Data.Repository;
using Ulx.Data.Repository.IRepository;
using Ulx.Models;

namespace Ulx.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categories;

        public CategoryController(ICategoryRepository categories)
        {
            _categories = categories;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(_categories.GetAll(orderBy: c => c.OrderBy(r => r.Name)).ToList());
        }

        [HttpPost]
        public JsonResult Post(Category category)
        {
            if (_categories.Add(category) && _categories.Save())
            {
                return new JsonResult("Success");
            }
            return new JsonResult("Error adding category");
        }

        [HttpPut]
        public JsonResult Put(Category category)
        {
            Category cat = _categories.FirstOrDefault(filter: r => r.Id == category.Id);
            if (cat != null)
            {
                cat.Name = category.Name;
                if (_categories.Update(cat) && _categories.Save())
                {
                    return new JsonResult("Success");
                }
            }
            return new JsonResult("Error updating category");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            Category cat = _categories.FirstOrDefault(filter: r => r.Id == id);
            if (cat != null)
            {
                if (_categories.Remove(cat) && _categories.Save())
                {
                    return new JsonResult("Success");
                }
            }
            return new JsonResult("Error deleting category");
        }
    }
}
