using System.ComponentModel;

namespace My.WebApi.Dto
{
    public class WeatherForecast
    {
        [DisplayName("����")]
        public DateOnly Date { get; set; }

        [DisplayName("�����������")]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
