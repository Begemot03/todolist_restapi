using Serilog;

namespace API
{
    internal class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configurationProvider = new Configuration.Provider.ConfigurationProvider();
            var service = configurationProvider.AppSettings.Service;

            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => webBuilder
                .UseStartup<Startup>()
                .UseUrls($"http://*:{service.Port}")
                .UseConfiguration(configurationProvider.GetConfiguration()));
        }
        private static void Main(string[] args)
        {
            var configurationProvider = new Configuration.Provider.ConfigurationProvider();
            var service = configurationProvider.AppSettings.Service;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Hour)
                .CreateLogger();

            try
            {
                Log.Information($"Initializing 'Example API' service on port '{service.Port}'");
            } 
            catch(Exception exception)
            {

            } 
            finally
            {

            }
        }
    }
}
