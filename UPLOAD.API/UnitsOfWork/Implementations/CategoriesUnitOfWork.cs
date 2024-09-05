using UPLOAD.API.Repositories.Implementations;
using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.UnitsOfWork.Implementations
{
    public class CategoriesUnitOfWork : GenericUnitOfWork<Category>, ICategoriesUnitOfWork
    {
       
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesUnitOfWork(IGenericRepository<Category> repository, ICategoriesRepository categoriesRepository) : base(repository)
        {
           
            _categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<Category>> GetComboAsync()=>await _categoriesRepository.GetComboAsync();
       
    }
}
