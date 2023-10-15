

namespace API.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
        .Property(e => e.DateOfBirth)
        .HasConversion(
            v => new DateTime(v.Year, v.Month, v.Day),  // Convert DateOnly to DateTime
            v => new DateOnly(v.Year, v.Month, v.Day)   // Convert DateTime back to DateOnly
        );
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Photo> Photos { get; set; }

    }
}
