using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Models
{
    public class IdentityTestDBContext : IdentityDbContext
    {

        public IdentityTestDBContext(DbContextOptions<IdentityTestDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

    }
}
