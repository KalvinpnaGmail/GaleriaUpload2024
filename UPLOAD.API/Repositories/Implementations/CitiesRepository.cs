using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.API.Helpers;
using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Repositories.Implementations
{
    public class CitiesRepository : GenericRepository<City>, ICitiesRepository
    {
        private readonly DataContext _context;

        public CitiesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<IEnumerable<City>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Cities
               .Where(x => x.Provincia!.Id == pagination.Id)
               .AsQueryable();

            return new ActionResponse<IEnumerable<City>>
            {
                WasSuccess = true,
                Result = await queryable
                .OrderBy(c => c.Name)
                .Paginate(pagination)
                .ToListAsync()

            };

        }

        public async Task<IEnumerable<City>> GetComboAsync(int provinciaId)
        {
            return await _context.Cities
                .Where(c=> c.ProvinciaId== provinciaId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public override async  Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Cities
                 .Where(x => x.Provincia!.Id == pagination.Id)
                 .AsQueryable();
            var count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((double)count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages,
            };
            
        }
    }

}

