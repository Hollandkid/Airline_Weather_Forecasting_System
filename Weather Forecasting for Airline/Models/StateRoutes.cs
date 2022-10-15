using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather_Forecasting_for_Airline.Models
{
    public class StateRoutes
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string InterState { get; set; }


    }
}
