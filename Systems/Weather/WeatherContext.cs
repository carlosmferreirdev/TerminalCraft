namespace TerminalCraft.Systems.Weather
{
    public class WeatherContext
    {
        public double TemperatureC { get; set; }
        public double PrecipitationMm { get; set; }
        public string SourceSummary { get; set; } = string.Empty;

        public bool IsCold => TemperatureC <= 0;
        public bool IsCool => TemperatureC > 0 && TemperatureC < 15;
        public bool IsWarm => TemperatureC >= 15 && TemperatureC < 25;
        public bool IsHot => TemperatureC >= 25;
        public bool IsWet => PrecipitationMm > 2.5; // arbitrary threshold
        public override string ToString() => $"Temp: {TemperatureC:F1}C, Precip: {PrecipitationMm:F1}mm";
    }
}
