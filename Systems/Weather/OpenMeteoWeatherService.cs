using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TerminalCraft.Systems.Weather
{
    public class OpenMeteoWeatherService : IWeatherService
    {
        private readonly HttpClient _http;
        public OpenMeteoWeatherService(HttpClient? httpClient = null)
        {
            _http = httpClient ?? new HttpClient();
            _http.Timeout = TimeSpan.FromSeconds(5);
        }

        public async Task<WeatherContext> GetWeatherAsync(double latitude, double longitude)
        {
            return await GetWeatherFromCoordinatesAsync(latitude, longitude);
        }

        public async Task<WeatherContext> GetWeatherAsync(string cityName)
        {
            var ctx = new WeatherContext();
            try
            {
                // First, geocode the city name to get coordinates
                var coordinates = await GeocodeAsync(cityName);
                if (coordinates == null)
                {
                    ctx.SourceSummary = $"City '{cityName}' not found";
                    return ctx;
                }

                // Then get weather for those coordinates
                ctx = await GetWeatherFromCoordinatesAsync(coordinates.Value.lat, coordinates.Value.lon);
                ctx.SourceSummary = $"Open-Meteo ({cityName})";
                return ctx;
            }
            catch (Exception ex)
            {
                ctx.SourceSummary = "Error: " + ex.GetType().Name;
                return ctx;
            }
        }

        private async Task<(double lat, double lon)?> GeocodeAsync(string cityName)
        {
            try
            {
                string url = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(cityName)}&count=1&language=en&format=json";
                using var resp = await _http.GetAsync(url);
                if (!resp.IsSuccessStatusCode) return null;

                using var stream = await resp.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                if (doc.RootElement.TryGetProperty("results", out var results) &&
                    results.ValueKind == JsonValueKind.Array &&
                    results.GetArrayLength() > 0)
                {
                    var first = results[0];
                    if (first.TryGetProperty("latitude", out var latElement) &&
                        first.TryGetProperty("longitude", out var lonElement) &&
                        latElement.TryGetDouble(out double lat) &&
                        lonElement.TryGetDouble(out double lon))
                    {
                        return (lat, lon);
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private async Task<WeatherContext> GetWeatherFromCoordinatesAsync(double latitude, double longitude)
        {
            var ctx = new WeatherContext();
            try
            {
                // Using open-meteo.com free API (no key required)
                // We'll fetch current temperature and precipitation (24h sum)
                string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current=temperature_2m&daily=precipitation_sum&timezone=UTC";
                using var resp = await _http.GetAsync(url);
                if (!resp.IsSuccessStatusCode)
                {
                    ctx.SourceSummary = $"Failed HTTP ({resp.StatusCode})";
                    return ctx; // return defaults
                }
                using var stream = await resp.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                // Parse temperature
                if (doc.RootElement.TryGetProperty("current", out var current) &&
                    current.TryGetProperty("temperature_2m", out var tempElement) &&
                    tempElement.TryGetDouble(out double temp))
                {
                    ctx.TemperatureC = temp;
                }
                // Parse precipitation (daily sum first item)
                if (doc.RootElement.TryGetProperty("daily", out var daily) &&
                    daily.TryGetProperty("precipitation_sum", out var precipArray) &&
                    precipArray.ValueKind == JsonValueKind.Array &&
                    precipArray.GetArrayLength() > 0 &&
                    precipArray[0].TryGetDouble(out double precip))
                {
                    ctx.PrecipitationMm = precip;
                }
                ctx.SourceSummary = "Open-Meteo";
            }
            catch (Exception ex)
            {
                ctx.SourceSummary = "Error: " + ex.GetType().Name;
            }
            return ctx;
        }
    }
}
