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
    public class PictureRepository : Repository<Picture>, IPictureRepository
    {
        private readonly UlxDbContext _db;
        private readonly ILogger<Repository<Picture>> _logger;

        public PictureRepository(UlxDbContext db, ILogger<Repository<Picture>> logger) : base(db, logger)
        {
            _db = db;
            _logger = logger;
        }

        public Picture Find(int id)
        {
            Picture Picture = _db.Pictures.Find(id);
            return Picture;
        }

    }}
