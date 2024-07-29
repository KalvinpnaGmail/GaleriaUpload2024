using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Repositories.Implementations
{
    public class CountryRepository :GenericRepository<Country>, ICountriesRepository
    {
        private readonly DataContext _contex;

        public CountryRepository(DataContext contex) : base(contex)
        {
          _contex = contex;
        }

        public override async Task<ActionResponse<Country>> GetAsync(int id)
        {
            var country = await _contex.Countries
                .Include(c => c.Provincias!)
                .ThenInclude(s=>s.Cities)
                .FirstOrDefaultAsync(c => c.Id == id);  
            if (country==null)
            {
                return new ActionResponse<Country>
                {
                    WasSuccess = false,
                    Message = "Pais no Existe"
                };
            }
            return new ActionResponse<Country>
            {
                WasSuccess = true,
                Result = country
            };
           
        }

        //tengo que override asi sobreescriboe el GenericRepository
        public  override async Task<ActionResponse<IEnumerable<Country>>> GetAsync()
        {
           var countries = await _contex.Countries.Include(c=>c.Provincias).ToListAsync();
            return new ActionResponse<IEnumerable<Country>>
            {
                WasSuccess = true,
                Result = countries
            };

        }
    }
}
