using DotnetIdentityWebAPI.Models;
using DotnetIdentityWebAPI.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotnetIdentityWebAPI.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IConfiguration _configuration;

		//For creating a user, we need to make use of the UserManager class.
		//We need to tell the Identity, what implementation of user we are going to pass to it

		//Now we need to inject it in the Program.cs file
		public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
		{
			this._userManager = userManager;
			this._configuration = configuration;
		}



		public async Task<bool> RegisterUser(LoginUser user)
		{
			var identityUser = new IdentityUser()
			{
				UserName = user.UserName,
				Email = user.UserName
			};

			//The user manager will create the user and automatically hash the password
			var result = await _userManager.CreateAsync(identityUser, user.Password);
			//If the user is created successfully, it is going to return true and if not false.
			return result.Succeeded;


		}
		public async Task<bool> Login(LoginUser user)
		{
			//In Aspnetcore Identity , first we need to get the user from the database and check the password

			var identityUser = await _userManager.FindByEmailAsync(user.UserName);
			//No user found if identity is null
			if (identityUser is null)
			{
				return false;
			}

			//Check whether the password is correct or not
			return await _userManager.CheckPasswordAsync(identityUser, user.Password);
		}

		public string GenerateTokenString(LoginUser user)
		{
			//claims are some information which we can place inside the token
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Email,user.UserName),
				new Claim(ClaimTypes.Role,"Admin"),
			};
			SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
			//SigningCredentials will make sure that the jwt is not tampered by anyone
			SigningCredentials signingCred = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);
			var securityToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(60), issuer: _configuration.GetSection("Jwt:Issuer").Value, audience: _configuration.GetSection("Jwt:Audience").Value, signingCredentials: signingCred);
			string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
			return tokenString;
		}
	}
}
