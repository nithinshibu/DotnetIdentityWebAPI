using DotnetIdentityWebAPI.DbContext;
using DotnetIdentityWebAPI.ServiceContracts;
using DotnetIdentityWebAPI.Services;
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

app.UseAuthorization();

app.MapControllers();

app.Run();
