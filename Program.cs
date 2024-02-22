using DotnetIdentityWebAPI.DbContext;
using DotnetIdentityWebAPI.ServiceContracts;
using DotnetIdentityWebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AuthDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value);
});

//Inside the identity we need to pass two types: first for the User and then next for the Roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
	//We can provide some options into this
	options.Password.RequiredLength = 5;
	//Then we need to tell it that this Identity is going to use our Entity Framework and the DbContext.
	//Then we need to tell it what kind of token we are going to use
}).AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
	//DefaultAuthenticateScheme specifies the authentication scheme that will be used to authenticate incoming requests by default. This means that when a request requires authentication, ASP.NET Core will attempt to authenticate it using the specified scheme unless another scheme is explicitly specified.

	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

	//DefaultChallengeScheme defines the authentication scheme that will be used to challenge unauthorized requests. When an unauthenticated request accesses a resource requiring authentication, ASP.NET Core will automatically issue a challenge response using the specified scheme, prompting the client to provide credentials or authenticate.

	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateActor=true,
		// The "issuer" claim identifies the principal that issued the JWT. It typically represents the entity that generated the token. The issuer is usually a server or service responsible for authenticating users and generating tokens. For example, if you're using a service like Auth0 or Azure Active Directory to handle authentication and token issuance, the issuer would be the URL of that service.
		ValidateIssuer = true,
		//The "audience" claim identifies the recipients for which the JWT is intended. It specifies the intended audience that the JWT is meant for. This can be the specific application or service that should accept the token. When validating a JWT, the audience claim is typically checked to ensure that the token is intended for the recipient. For instance, if your application consumes tokens from a specific authorization server, the audience claim would contain the identifier of your application or service.
		ValidateAudience = true,
		RequireExpirationTime=true,
		ValidateIssuerSigningKey=true,
		//Since we have set the validate issuer and validate audience as true , we need to tell which are the valid issuer and audience
		ValidIssuer=builder.Configuration.GetSection("Jwt:Issuer").Value,
		ValidAudience= builder.Configuration.GetSection("Jwt:Audience").Value
	};
});



builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Make sure that the UseAuthentication() middleware comes before UseAuthorization()
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
