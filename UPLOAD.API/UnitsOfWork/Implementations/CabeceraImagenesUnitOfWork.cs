using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.UnitsOfWork.Implementations
{
    public class CabeceraImagenesUnitOfWork : GenericUnitOfWork<CabeceraImage>, ICabeceraImagenesUnitOfWork
    {
        private readonly ICabeceraImagenesRepository _cabeceraImagenesRepository;

        public CabeceraImagenesUnitOfWork(IGenericRepository<CabeceraImage> repository, ICabeceraImagenesRepository cabeceraImagenesRepository) : base(repository)
        {
            _cabeceraImagenesRepository = cabeceraImagenesRepository;
        }

        public async Task<ActionResponse<IEnumerable<CabeceraImage>>> GetAsync() => await _cabeceraImagenesRepository.GetAsync();

        public async Task<ActionResponse<CabeceraImage>> GetAsync(int id) => await _cabeceraImagenesRepository.GetAsync((id));

        public async Task<ActionResponse<CabeceraImage>> AddAsync(CabeceraImage cabeceraImage) =>
    await _cabeceraImagenesRepository.AddAsync(cabeceraImage);
    }
}