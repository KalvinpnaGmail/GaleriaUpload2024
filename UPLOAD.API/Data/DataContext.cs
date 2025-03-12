using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.SetCommandTimeout(600);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Provincia> Provincias { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<CabeceraImage> CabeceraImages { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<City>().HasIndex(x => new { x.ProvinciaId, x.Name }).IsUnique();
            modelBuilder.Entity<Provincia>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();
            modelBuilder.Entity<CabeceraImage>()
            .HasMany(c => c.Images)
            .WithOne()
            .HasForeignKey(i => i.CabeceraImageId); // Suponiendo que tienes un campo CabeceraImageId en Image

            //modelBuilder.ApplyConfiguration(new ImageConfiguration());
            //modelBuilder.ApplyConfiguration(new CountryConfiguration());
            //modelBuilder.ApplyConfiguration(new CategoryConfiguraration());
            //modelBuilder.ApplyConfiguration(new ProvinciaConfiguration());
            //modelBuilder.ApplyConfiguration(new CityConfiguration());

            DisableCascadingDelete(modelBuilder);
        }

        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}