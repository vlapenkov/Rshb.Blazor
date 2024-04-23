using System.ComponentModel;

namespace Suap.Triast.Dto
{
    public class WeatherForecast
    {
        [DisplayName("Дата")]
        public DateOnly Date { get; set; }

        [DisplayName("Температура")]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
