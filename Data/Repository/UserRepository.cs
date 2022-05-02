
using Microsoft.Extensions.Logging;
using Ulx.Data.Repository.IRepository;
using Ulx.Models;

namespace Ulx.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly UlxDbContext _db;
        private readonly ILogger<Repository<User>> _logger;

        public UserRepository(UlxDbContext db, ILogger<Repository<User>> logger) : base(db, logger)
        {
            _db = db;
            _logger = logger;
        }

        public User Find(string id)
        {
            User user = _db.Users.Find(id);
            return user;
        }
    }
}
