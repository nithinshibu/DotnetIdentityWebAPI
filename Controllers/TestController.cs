using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotnetIdentityWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//Here I am adding the Authorize attribute on class level, also we can add them in method level.
	[Authorize]
	public class TestController : ControllerBase
	{
		//We want this method available only to authenticated users
		[HttpGet]

		public string Get()
		{
			return "Hello World";
		}
	}
}
