using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Repositories.Implementations
{
    public class ProvinciasRepository : GenericRepository<Provincia>, IProvinciasRepository
    {
        private readonly DataContext _contex;

        public ProvinciasRepository(DataContext contex) : base(contex)
        {
           _contex = contex;
        }

        public override async Task<ActionResponse<Provincia>> GetAsync(int id)
        {
           
            var provincia = await _contex.Provincias
                .Include(c => c.Cities)
                .FirstOrDefaultAsync(c => c.Id == id);  
            if (provincia==null)
            {
                return new ActionResponse<Provincia>
                {
                    WasSuccess = false,
                    Message = "Provincia no Existe"
                };
            }
            return new ActionResponse<Provincia>
            {
                WasSuccess = true,
                Result = provincia
            };
        }

        public override async Task<ActionResponse<IEnumerable<Provincia>>> GetAsync()
        {
            var provincias = await _contex.Provincias.Include(c => c.Cities).ToListAsync();
            return new ActionResponse<IEnumerable<Provincia>>
            {
                WasSuccess = true,
                Result = provincias
            };

        }
    }
}
