using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.UnitsOfWork.Implementations
{
    public class ProvinciasUnitOfWork : GenericUnitOfWork<Provincia>, IProvinciasUnitOfWork
    {
        private readonly IProvinciasRepository _provinciasRepository;

        public ProvinciasUnitOfWork(IGenericRepository<Provincia> repository, IProvinciasRepository provinciasRepository) : base(repository)
        {
           _provinciasRepository = provinciasRepository;
        }


       
        public override async Task<ActionResponse<IEnumerable<Provincia>>> GetAsync() => await _provinciasRepository.GetAsync();


        //para 
        public override async Task<ActionResponse<Provincia>> GetAsync(int id) => await _provinciasRepository.GetAsync(id);

        public override async Task<ActionResponse<IEnumerable<Provincia>>> GetAsync(PaginationDTO pagination) => await _provinciasRepository.GetAsync(pagination);

        public  async Task<IEnumerable<Provincia>> GetComboAsync(int countryId) => await _provinciasRepository.GetComboAsync(countryId);
     
        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _provinciasRepository.GetTotalPagesAsync(pagination);



    }
}
