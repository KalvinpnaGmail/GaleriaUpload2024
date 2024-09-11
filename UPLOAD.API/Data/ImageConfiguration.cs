using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> entity)
        {
        


            // Configuración de la clave primaria (Id)
            entity.HasKey(x => x.Id);

            // Configura la columna Id como Identity
            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd(); // Asegura que Id es generado automáticamente por la base de datos





            entity.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);


            

            entity.Property(x => x.Url)
                .IsRequired()
                .HasColumnType("text");
        }


     





    }

   
}
