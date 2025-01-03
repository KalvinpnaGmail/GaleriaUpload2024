using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> entity)
        {
            // Configuración para el índice único en 'Name'
            //no me va a permitir crear dos sana benito den Ciudadd
            entity.HasIndex(x => new { x.ProvinciaId, x.Name }).IsUnique();

            entity.Property(x => x.Name)
             .IsRequired()
             .HasMaxLength(100);
        }
    }
}