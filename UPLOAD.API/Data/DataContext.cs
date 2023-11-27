using Microsoft.EntityFrameworkCore;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public partial class DataContext:DbContext
    {

        public virtual DbSet<Image> Images { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

            


        public DataContext() 
        { 
        }


             

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ImageConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }


       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => base.OnConfiguring(optionsBuilder);

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

