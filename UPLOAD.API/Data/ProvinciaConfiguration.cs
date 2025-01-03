using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public class ProvinciaConfiguration : IEntityTypeConfiguration<Provincia>
    {
        public void Configure(EntityTypeBuilder<Provincia> entity)
        {
            // Configuración para el índice único en 'Name'
            //una provincia
            entity.HasIndex(x => new { x.CountryId, x.Name }).IsUnique();

            entity.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);
        }
    }
}