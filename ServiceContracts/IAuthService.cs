using DotnetIdentityWebAPI.Models;

namespace DotnetIdentityWebAPI.ServiceContracts
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(LoginUser user);
    }
}