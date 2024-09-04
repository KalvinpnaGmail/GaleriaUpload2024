using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.UnitsOfWork.Interfaces
{
    public interface ICategoriesUnitOfWork
    {
        Task<IEnumerable<Category>> GetComboAsync();
    }
}
