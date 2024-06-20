using ECommerceProject.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProject.Services.EmailAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EmailLogger> EmailLoggers { get; set; }


    }
}
