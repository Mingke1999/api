using Microsoft.EntityFrameworkCore;
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<Stock> Stocks {get; set;}
        public DbSet<Comment> Comments {get; set;}
    }
}