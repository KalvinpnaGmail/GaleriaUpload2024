using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Repositories.Implementations
{
    public class CabeceraImagenesRepository : GenericRepository<CabeceraImage>, ICabeceraImagenesRepository
    {
        private readonly DataContext _contex;

        public CabeceraImagenesRepository(DataContext contex) : base(contex)
        {
            _contex = contex;
        }

        public override async Task<ActionResponse<CabeceraImage>> AddAsync(CabeceraImage cabeceraImage)
        {
            if (cabeceraImage == null)
            {
                return new ActionResponse<CabeceraImage>
                {
                    WasSuccess = false,
                    Message = "Los datos de la cabecera son inválidos."
                };
            }

            // Agregar la cabecera primero
            _contex.CabeceraImages.Add(cabeceraImage);

            try
            {
                await _contex.SaveChangesAsync(); // Guardamos para obtener el ID

                // Verificar si hay imágenes antes de intentar agregarlas
                if (cabeceraImage.Images != null && cabeceraImage.Images.Any())
                {
                    foreach (var image in cabeceraImage.Images)
                    {
                        image.CabeceraImageId = cabeceraImage.Id; // Asignar el ID generado
                        _contex.Images.Add(image);
                    }

                    await _contex.SaveChangesAsync(); // Guardamos las imágenes
                }

                return new ActionResponse<CabeceraImage>
                {
                    WasSuccess = true,
                    Result = cabeceraImage
                };
            }
            catch (DbUpdateException ex)
            {
                return new ActionResponse<CabeceraImage>
                {
                    WasSuccess = false,
                    Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }

        private ActionResponse<CabeceraImage> ExceptionActionResponse(Exception exception)
        {
            return new ActionResponse<CabeceraImage>
            {
                WasSuccess = false,
                Message = $"Error inesperado: {exception.Message}"
            };
        }

        private ActionResponse<CabeceraImage> DbUpdateExceptionActionResponse()
        {
            return new ActionResponse<CabeceraImage>
            {
                WasSuccess = false,
                Message = "Error al guardar la cabecera de imágenes. Posible duplicado o problema en la base de datos."
            };
        }

        public override async Task<ActionResponse<IEnumerable<CabeceraImage>>> GetAsync()
        {
            var cabecera = await _contex.CabeceraImages
                .OrderBy(x => x.ObraSocial)
                .ToListAsync();
            return new ActionResponse<IEnumerable<CabeceraImage>>
            {
                WasSuccess = true,
                Result = cabecera
            };
        }

        public override async Task<ActionResponse<CabeceraImage>> GetAsync(int id)

        {
            var cabecera = await _contex.CabeceraImages
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cabecera == null)
            {
                return new ActionResponse<CabeceraImage>
                {
                    WasSuccess = false,
                    Message = "Cabecera no Encontrada"
                };
            }

            return new ActionResponse<CabeceraImage>
            {
                WasSuccess = true,
                Result = cabecera
            };
        }
    }
}