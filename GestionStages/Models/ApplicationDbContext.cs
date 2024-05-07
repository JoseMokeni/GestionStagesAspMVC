using Microsoft.EntityFrameworkCore;

namespace GestionStages.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Internship> Internships { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Internship>().ToTable("Internships");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<Group>().ToTable("Groups");
        }

    }
}
