using DotnetIdentityWebAPI.DbContext;
using DotnetIdentityWebAPI.ServiceContracts;
using DotnetIdentityWebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
