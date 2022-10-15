using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather_Forecasting_for_Airline.Models
{
    public class StateVM
    {
        public List<States> states { get; set; }
        public List<StateRoutes> stateRoutes { get; set; }
        public List<WeatherLog> WeatherLogs { get; set; }

        public string  stateFrom { get; set; }
        public string  stateTo { get; set; }
        public string allRoute { get; set; }
        public string RouteId { get; set; }

    }
}
