using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public class ProvinciaConfiguration:IEntityTypeConfiguration<Provincia>
    {
        public void Configure(EntityTypeBuilder<Provincia> entity)
        {
            entity.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);


            // Configuración para el índice único en 'Name'
            entity.HasIndex(x => x.Name).IsUnique();




        }

    }
}
