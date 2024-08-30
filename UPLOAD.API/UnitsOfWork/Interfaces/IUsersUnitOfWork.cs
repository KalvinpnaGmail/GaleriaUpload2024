using Microsoft.AspNetCore.Identity;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.UnitsOfWork.Interfaces
{
    public interface IUsersUnitOfWork
    {
        Task<User> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);


        //para loguearse
        Task<SignInResult> LoginAsync(LoginDTO model);

        //desloguearse
        Task LogoutAsync();

    }
}
