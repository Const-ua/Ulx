using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.Logging;
using Ulx.Data.Repository.IRepository;
using Ulx.Models;

namespace Ulx.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly UlxDbContext _db;
        private readonly ILogger<Repository<Category>> _logger;

        public CategoryRepository(UlxDbContext db, ILogger<Repository<Category>> logger) : base(db, logger)
        {
            _db = db;
            _logger = logger;
        }

        public Category Find(int id)
        {
            Category Category = _db.Categories.Find(id);
            return Category;
        }

    }
}
