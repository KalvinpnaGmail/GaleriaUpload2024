using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public class CabeceraImageConfiguration : IEntityTypeConfiguration<CabeceraImage>
    {
        public void Configure(EntityTypeBuilder<CabeceraImage> entity)
        {
            // Configuración de la clave primaria (Id)
            entity.HasKey(x => x.Id);

            // Configura la columna Id como Identity
            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd(); // Asegura que Id es generado automáticamente por la base de datos


        }
    }
}
