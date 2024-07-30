using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.API.UnitsOfWork.Interfaces;
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



        public override async Task<ActionResponse<Provincia>> GetAsync(int id) => await _provinciasRepository.GetAsync(id);

    }
}
