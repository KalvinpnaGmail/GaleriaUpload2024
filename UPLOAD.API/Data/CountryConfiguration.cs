using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> entity)
        {

            // Configuración de la clave primaria (Id)
            entity.HasKey(x => x.Id);

            // Configura la columna Id como Identity
            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd(); // Asegura que Id es generado automáticamente por la base de datos






            entity.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);


            // Configuración para el índice único en 'Name'
            entity.HasIndex(x =>new {x.Id, x.Name }).IsUnique();



          
        }

      
    }
}
