using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Repositories.Implementations
{
    public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
    {
        private readonly DataContext _contex;

        public CategoriesRepository(DataContext contex) : base(contex)
        {
            _contex = contex;
        }

        public async Task<IEnumerable<Category>> GetComboAsync()
        {
            return await _contex.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

    }
}
