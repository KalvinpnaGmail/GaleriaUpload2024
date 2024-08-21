using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public class CityConfiguration:IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> entity)
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
            //no me va a permitir crear dos sana benito den Ciudadd
            entity.HasIndex(x =>new {x.ProvinciaId, x.Name }).IsUnique();
            




        }

    }
}
