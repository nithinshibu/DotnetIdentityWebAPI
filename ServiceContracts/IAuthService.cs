using DotnetIdentityWebAPI.Models;

namespace DotnetIdentityWebAPI.ServiceContracts
{
    public interface IAuthService
    {		
		Task<bool> Login(LoginUser user);
		Task<bool> RegisterUser(LoginUser user);
    }
}