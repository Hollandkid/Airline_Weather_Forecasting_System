using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather_Forecasting_for_Airline.Models;

namespace Weather_Forecasting_for_Airline.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<States> States { get; set; }
        public DbSet<StateRoutes> StateRoutes { get; set; }
        public DbSet<WeatherLog> WeatherLog { get; set; }
    }
}
