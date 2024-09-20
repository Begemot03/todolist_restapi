using API.Configuration.Models;

namespace API.Configuration
{
    public sealed class AppSettings
    {
        public DatabaseConfigurationModel Database { get; set; }
        public ServiceConfigurationModel Service { get; set; }
    }
}