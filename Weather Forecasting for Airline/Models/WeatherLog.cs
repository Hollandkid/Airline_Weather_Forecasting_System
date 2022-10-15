using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather_Forecasting_for_Airline.Models
{
    public class WeatherLog
    {
        public int Id { get; set; }
        public string WeatherId { get; set; }
        public string Name { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Pressure { get; set; }
        public string Humidity { get; set; }
        
        public int RouteId { get; set; }
    }
}
