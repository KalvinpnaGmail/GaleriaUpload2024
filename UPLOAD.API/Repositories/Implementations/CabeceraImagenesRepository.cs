using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
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

        public async Task<ActionResponse<IEnumerable<CabeceraImage>>> GetAsync()
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