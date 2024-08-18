using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.UnitsOfWork.Implementations
{
    public class CountriesUnitOfWork :GenericUnitOfWork<Country>, ICountriesUnitofWork
    {
       
        private readonly ICountriesRepository _countriesRepository;

        public CountriesUnitOfWork(IGenericRepository<Country> repository, ICountriesRepository countriesRepository) : base(repository)
        {
           
            _countriesRepository = countriesRepository;
        }



        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync() => await _countriesRepository.GetAsync();



        public override async Task<ActionResponse<Country>> GetAsync(int id) => await _countriesRepository.GetAsync(id);

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination) => await _countriesRepository.GetAsync(pagination);
    }
}
