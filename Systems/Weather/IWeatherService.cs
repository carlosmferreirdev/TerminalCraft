using System.Threading.Tasks;

namespace TerminalCraft.Systems.Weather
{
    public interface IWeatherService
    {
        Task<WeatherContext> GetWeatherAsync(double latitude, double longitude);
        Task<WeatherContext> GetWeatherAsync(string cityName);
    }
}
