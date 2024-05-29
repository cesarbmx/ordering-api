using CesarBmx.Ordering.Domain.Models;

namespace CesarBmx.Ordering.Application.Settings
{
    public class AppSettings : Shared.Settings.AppSettings
    {
        public bool UseMemoryStorage { get; set; }
        public int JobsIntervalInMinutes { get; set; }
        public string TelegramApiToken { get; set; }
    }
}
