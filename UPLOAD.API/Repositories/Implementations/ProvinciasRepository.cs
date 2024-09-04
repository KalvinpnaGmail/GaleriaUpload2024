using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.API.Helpers;
using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.SHARE.DTOS;
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
            var provincias = await _contex.Provincias
                .OrderBy(c => c.Name)
                .Include(c => c.Cities)
                .ToListAsync();
            return new ActionResponse<IEnumerable<Provincia>>
            {
                WasSuccess = true,
                Result = provincias
            };

        }



        public override async Task<ActionResponse<IEnumerable<Provincia>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _contex.Provincias
                .Include(x => x.Cities)
                .Where(x => x.Country!.Id == pagination.Id)
                .AsQueryable();

            return new ActionResponse<IEnumerable<Provincia>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<Provincia>> GetComboAsync(int countryId)
        {
           return await _contex.Provincias.Where(s=> s.CountryId==countryId)
                .OrderBy(x => x.Name)   
                .ToListAsync();
        }

        public async override Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _contex.Provincias
                .Where(x => x.Country!.Id == pagination.Id)
                .AsQueryable();

            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }





    }
}
