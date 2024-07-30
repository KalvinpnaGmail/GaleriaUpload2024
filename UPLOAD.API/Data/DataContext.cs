using Microsoft.EntityFrameworkCore;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {



        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Provincia> Provincias { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
       
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
        //}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //modelBuilder.Entity<Image>().HasIndex(x => x.Name).IsUnique();
            //modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguraration());
        }




    }
}





