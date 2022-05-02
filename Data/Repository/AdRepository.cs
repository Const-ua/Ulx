using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ulx.Data.Repository.IRepository;
using Ulx.Models;

namespace Ulx.Data.Repository
{
    public class AdRepository: Repository<Ad>, IAdRepository
    {
        private readonly UlxDbContext _db;
        private readonly ILogger<Repository<Ad>> _logger;

        public AdRepository (UlxDbContext db,ILogger<Repository<Ad>> logger):base (db, logger)
        {
            _db = db;
            _logger = logger;
        }

        public Ad Find(int id)
        {
            Ad ad = _db.Ads.Find(id);
            return ad;
        }
    }
}
