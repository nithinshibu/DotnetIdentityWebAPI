﻿using DotnetIdentityWebAPI.Models;
using DotnetIdentityWebAPI.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace DotnetIdentityWebAPI.Services
{
    public class AuthService : IAuthService
	{
		private readonly UserManager<IdentityUser> _userManager;

		//For creating a user, we need to make use of the UserManager class.
		//We need to tell the Identity, what implementation of user we are going to pass to it

		//Now we need to inject it in the Program.cs file
		public AuthService(UserManager<IdentityUser> userManager)
		{
			this._userManager = userManager;
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
	}
}