using System;
using System.Threading.Tasks;

namespace TerminalCraft.Systems.Weather
{
    public class RandomWeatherService : IWeatherService
    {
        private readonly Random _rand = new Random();
        private readonly IWeatherService _apiWeatherService;
        
        private readonly string[] _climateNames = 
        {
            "Arctic", "Cool", "Mild", "Warm", "Hot"
        };

        // One representative city per climate type
        private readonly string[] _globalCities = 
        {
            "Reykjavik",    // Arctic/Cold
            "London",       // Cool
            "Paris",        // Mild/Temperate
            "Barcelona",    // Warm/Mediterranean
            "Dubai",        // Hot/Arid
            "Bangkok"       // Hot/Tropical
        };

        public RandomWeatherService(IWeatherService? apiWeatherService = null)
        {
            _apiWeatherService = apiWeatherService ?? new OpenMeteoWeatherService();
        }

        public Task<WeatherContext> GetWeatherAsync(double latitude, double longitude)
        {
            // Generate random weather for fictional coordinates
            return Task.FromResult(GenerateRandomWeather());
        }

        public Task<WeatherContext> GetWeatherAsync(string cityName)
        {
            // Generate random weather for fictional city
            var weather = GenerateRandomWeather();
            weather.SourceSummary = $"Fictional ({cityName})";
            return Task.FromResult(weather);
        }

        public WeatherContext GenerateRandomWeather()
        {
            return GenerateRandomWeatherAsync().GetAwaiter().GetResult();
        }

        public async Task<WeatherContext> GenerateRandomWeatherAsync()
        {
            // Pick a random city from around the world
            string randomCity = _globalCities[_rand.Next(_globalCities.Length)];
            
            try
            {
                // Try to get real weather from the random city
                var weather = await _apiWeatherService.GetWeatherAsync(randomCity);
                
                // Check if we got valid data (non-default values)
                if (!string.IsNullOrEmpty(weather.SourceSummary) && 
                    !weather.SourceSummary.Contains("Error") && 
                    !weather.SourceSummary.Contains("Failed") &&
                    !weather.SourceSummary.Contains("not found"))
                {
                    // Use real weather but hide the actual city - make it feel like a fantasy realm
                    string climateName = GetClimateNameForTemp(weather.TemperatureC);
                    weather.SourceSummary = $"Regional Climate: {climateName}";
                    return weather;
                }
            }
            catch
            {
                // Fall through to generated weather if API fails
            }
            
            // Fallback to generated fictional weather
            return GenerateFictionalWeather();
        }

        public WeatherContext GenerateFictionalWeather(string? inspirationCity = null)
        {
            // Generate temperature between -20°C and 40°C with bias toward moderate temps
            double temp = GenerateWeightedTemperature();
            
            // Generate precipitation between 0-50mm, with bias toward lower values
            double precip = Math.Pow(_rand.NextDouble(), 2) * 50; // Square makes lower values more likely
            
            string climateName = GetClimateNameForTemp(temp);
            
            return new WeatherContext
            {
                TemperatureC = temp,
                PrecipitationMm = precip,
                SourceSummary = $"Generated Climate: {climateName}"
            };
        }

        private double GenerateWeightedTemperature()
        {
            // Use triangular distribution to favor moderate temperatures
            double u1 = _rand.NextDouble();
            double u2 = _rand.NextDouble();
            
            // Generate value between -1 and 1, then scale and shift
            double normalized = (u1 + u2 - 1.0);
            
            // Scale to -20 to 40°C range, with center around 15°C
            return 15 + (normalized * 25);
        }

        private string GetClimateNameForTemp(double temp)
        {
            return temp switch
            {
                <= 0 => _climateNames[0],      // Arctic
                <= 10 => _climateNames[1],     // Cool
                <= 20 => _climateNames[2],     // Mild
                <= 30 => _climateNames[3],     // Warm
                _ => _climateNames[4]          // Hot
            };
        }
    }
}