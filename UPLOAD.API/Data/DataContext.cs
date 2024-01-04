using Microsoft.EntityFrameworkCore;
using Sales.Shared.Entities;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public class DataContext : DbContext
    {



        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Country> Countries { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
        //}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
//modelBuilder.Entity<Image>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
        }


       

    }
}



    

