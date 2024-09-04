using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Repositories.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetComboAsync();

    }
}
