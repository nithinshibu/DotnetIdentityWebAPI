using DotnetIdentityWebAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotnetIdentityWebAPI.DbContext
{
	public class AuthDbContext:IdentityDbContext
	{
        public AuthDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Employee> Employee {  get; set; }  
    }
}
