using Microsoft.EntityFrameworkCore;

namespace WebAPIs.Models
{
    public class JWTDBContext : DbContext
    {

        //public JWTDBContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local);database =JWTDB;uid=sa;pwd=123;Encrypt=False;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity => { entity.HasIndex(x => x.UserName).IsUnique(); });
            modelBuilder.Entity<User>()
                           .HasMany(u => u.Roles)
                           .WithMany(r => r.Users)
                           .UsingEntity(j => j.ToTable("UserRoles"));

        }
    }
}
