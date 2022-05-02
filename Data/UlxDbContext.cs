using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ulx.Models;

namespace Ulx.Data
{
    public class UlxDbContext : IdentityDbContext<User>
    {

        public UlxDbContext(DbContextOptions<UlxDbContext> options):base (options)
        {
            // Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Picture> Pictures { get; set; }

    }
}
