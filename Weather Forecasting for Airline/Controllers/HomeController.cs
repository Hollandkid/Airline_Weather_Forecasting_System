using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Weather_Forecasting_for_Airline.Data;
using Weather_Forecasting_for_Airline.Models;
using Weather_Forecasting_for_Airline.Models.DTOs;

namespace Weather_Forecasting_for_Airline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _dbContext;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            StateVM model = new StateVM();
            var result = _dbContext.States.ToList();
            var resultRoute = _dbContext.StateRoutes.ToList();

            model.states = result;
            model.stateRoutes = resultRoute;

            return View(model);
        }

        [HttpPost]
        public async Task<BaseResponse> AddRoute([FromBody] StateVM model)
        {
            if (model.stateFrom != null & model.stateTo != null && model.allRoute != null)
            {
                StateRoutes newRoute = new StateRoutes()
                {
                    From = model.stateFrom,
                    To = model.stateTo,
                    InterState = model.allRoute
                };

                var cState = await _dbContext.StateRoutes.AddAsync(newRoute);
                await _dbContext.SaveChangesAsync();
                return new BaseResponse(true, "Route Added Successfully");

                //return RedirectToAction("Index");
            }
            return new BaseResponse(false, "Unable to Add Route");

            //return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> CheckWeather([FromBody] StateVM model)
        {
            var weatherModel = new List<WeatherLog>();
            if (model.RouteId != null)
            {
                var getRoute = _dbContext.StateRoutes.FirstOrDefault(m => m.Id == Int32.Parse(model.RouteId));


                if (getRoute != null)
                {
                    string[] ObjRouteList = JsonConvert.DeserializeObject<string[]>(getRoute.InterState);

                    foreach (var route in ObjRouteList)
                    {
                        //check weather
                        var responseResult = await WeatherApi(route);
                        var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(responseResult);

                        var newModel = new WeatherLog()
                        {
                            WeatherId = rawWeather.Weather.Id,
                            Description = rawWeather.Weather.Description,
                            Icon= rawWeather.Weather.Icon,
                            Main= rawWeather.Weather.Main,
                            Humidity= rawWeather.Main.Humidity,
                            Pressure = rawWeather.Main.Pressure,
                            Name=rawWeather.Name,
                            RouteId= getRoute.Id

                        };
                        weatherModel.Add(newModel);
                        await _dbContext.WeatherLog.AddAsync(newModel);

                    }
                }
                var responseResult3 = await WeatherApi(getRoute.From);
                var rawWeather3 = JsonConvert.DeserializeObject<OpenWeatherResponse>(responseResult3);

                var newModel3 = new WeatherLog()
                    {
                        WeatherId = rawWeather3.Weather.Id,
                        Description = rawWeather3.Weather.Description,
                        Icon = rawWeather3.Weather.Icon,
                        Main = rawWeather3.Weather.Main,
                        Humidity = rawWeather3.Main.Humidity,
                        Pressure = rawWeather3.Main.Pressure,
                        Name = rawWeather3.Name,
                        RouteId = getRoute.Id

                    };
                    weatherModel.Add(newModel3);
                    await _dbContext.WeatherLog.AddAsync(newModel3);



                string responseResult2 = await WeatherApi(getRoute.To);
                    var rawWeather2 = JsonConvert.DeserializeObject<OpenWeatherResponse>(responseResult2);

                    var newModel2 = new WeatherLog()
                    {
                        WeatherId = rawWeather2.Weather.Id,
                        Description = rawWeather2.Weather.Description,
                        Icon= rawWeather2.Weather.Icon,
                        Main= rawWeather2.Weather.Main,
                        Humidity= rawWeather2.Main.Humidity,
                        Pressure = rawWeather2.Main.Pressure,
                        Name=rawWeather2.Name,
                        RouteId= getRoute.Id

                    };
                    weatherModel.Add(newModel2);
                    await _dbContext.WeatherLog.AddAsync(newModel2);
                
                await _dbContext.SaveChangesAsync();


                model = new StateVM();
                var result = _dbContext.States.ToList();
                var resultRoute = _dbContext.StateRoutes.ToList();

                model.states = result;
                model.stateRoutes = resultRoute;
                model.WeatherLogs = weatherModel;
                model.RouteId = getRoute.Id.ToString();
                model.stateFrom = getRoute.From;
                model.stateTo = getRoute.To;

                return View("index",model);


                //return new BaseResponse(true, "Route Added Successfully");

            }
            return RedirectToAction("index");

            //return RedirectToAction("Index");
        }

        public class OpenWeatherResponse
        {
            public string Name { get; set; }

            public WeatherDescription Weather { get; set; }

            public Main Main { get; set; }
        }

        public class WeatherDescription
        {
            public string Id { get; set; }
            public string Icon { get; set; }
            public string Main { get; set; }
            public string Description { get; set; }
        }

        public class Main
        {
            public string Temp { get; set; }
            public string Humidity { get; set; }
            public string Pressure { get; set; }
        }



        public static async Task<string> WeatherApi(string location)
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://open-weather13.p.rapidapi.com/city/"+location),
                Headers =
                    {
                        { "X-RapidAPI-Key", "e78331c71bmsh927e2a46e4b79bdp1c3364jsn79de47f8c800" },
                        { "X-RapidAPI-Host", "open-weather13.p.rapidapi.com" },
                    },
            };
            var body = "";
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                return body;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
