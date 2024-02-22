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
        [HttpPost("Register")]
		public async Task<IActionResult> RegisterUser(LoginUser user)
		{
            if (await _authService.RegisterUser(user))
            {
				return Ok("User Registered Successfully");
			}
			return BadRequest("User Registeration Failed");
            
		}
		
		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginUser user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			
			if (await _authService.Login(user))
			{
				return Ok("Login Success");
			}

			return BadRequest("Login Failed");
		}
	}
}
