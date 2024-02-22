using DotnetIdentityWebAPI.Models;

namespace DotnetIdentityWebAPI.ServiceContracts
{
    public interface IAuthService
    {
		string GenerateTokenString(LoginUser user);
		Task<bool> Login(LoginUser user);
		Task<bool> RegisterUser(LoginUser user);
    }
}