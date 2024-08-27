using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Xml.Linq;

namespace UPLOAD.WEB.AuthenticationProviders
{
    public class AuthenticationProviderTest:AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(500);
            var anonimous = new ClaimsIdentity();
            var user = new ClaimsIdentity(authenticationType: "test");
            var admin = new ClaimsIdentity(new List<Claim>
            {
                new Claim("FirstName", "Gabriel"),
                new Claim("LastName", "Lopez"),
                new Claim(ClaimTypes.Name,"lopez.gabriel@yopmail.com"),
                new Claim(ClaimTypes.Role, "Admin")
            },
            authenticationType:"test");
            
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
        }

    }
}
