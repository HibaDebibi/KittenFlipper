using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KittenFlipper.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KittenFlipper.Infrastructure
{
    public class DataBaseContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataBaseContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString(Constants.Constants.DefaultConnection));
        }

        public DbSet<User> Users { get; set; }
    }
}
