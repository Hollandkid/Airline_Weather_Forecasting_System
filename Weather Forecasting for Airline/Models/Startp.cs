using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather_Forecasting_for_Airline.Data;

namespace Weather_Forecasting_for_Airline.Models
{
    public class Startp
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var _state = serviceProvider.GetService<States>();
            var _context = serviceProvider.GetService<ApplicationDbContext>();

            string[] states = new string[] {"Abia", "Adamawa","Akwa Ibom","Anambra","Bauchi","Bayelsa","Benue","Borno","Cross River","Delta","Ebonyi","Edo","Ekiti",  "Enugu","FCT - Abuja","Gombe","Imo","Jigawa","Kaduna","Kano","Katsina","Kebbi","Kogi","Kwara","Lagos","Nasarawa","Niger","Ogun","Ondo","Osun","Oyo",  "Plateau","Rivers","Sokoto","Taraba","Yobe","Zamfara" };

            foreach (string state in states)
            {
                var isExist = _context.States.FirstOrDefault(s=> s.Name.ToLower().Equals(state.ToString().ToLower()));
                if (isExist == null)
                {
                    States newState = new States()
                    {
                        Name = state
                    };
                    var cState = await _context.States.AddAsync(newState);
                }

            }

            await _context.SaveChangesAsync();

            //await context.SaveChangesAsync();
        }


    }
}
