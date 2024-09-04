using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.UnitsOfWork.Implementations
{
    public class CitiesUnitOfWork : GenericUnitOfWork<City>, ICitiesUnitOfWork
    {
        private readonly ICitiesRepository _citiesRepository;

        public CitiesUnitOfWork(IGenericRepository<City> repository, ICitiesRepository citiesRepository) : base(repository)
        {
            _citiesRepository = citiesRepository;
        }

        public override async Task<ActionResponse<IEnumerable<City>>> GetAsync(PaginationDTO pagination)=>await _citiesRepository.GetAsync(pagination);

        public async Task<IEnumerable<City>> GetComboAsync(int provinciaId)=> await _citiesRepository.GetComboAsync(provinciaId);
        

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)=>await _citiesRepository.GetTotalPagesAsync(pagination);
    }
}
