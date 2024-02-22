using DotnetIdentityWebAPI.Models;
using DotnetIdentityWebAPI.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotnetIdentityWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		//We need to inject the Auth Service in the constructor
		public AuthController(IAuthService authService)
        {
			this._authService = authService;
		}
        [HttpPost]
		public async Task<bool> RegisterUser(LoginUser user)
		{
			return await _authService.RegisterUser(user);
		}
		
		[HttpGet]
		public async Task Login(LoginUser user)
		{

		}
	}
}
