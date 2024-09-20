namespace API.Configuration.Provider
{
    public sealed class ConfigurationProvider : IConfigurationProvider
    {
        private IConfiguration _configurationRoot;

        public AppSettings AppSettings { get; } = new();

        public ConfigurationProvider()
        {
            if(_configurationRoot == null)
            {
                BuildConfiguration();
            }
        }

        public IConfiguration GetConfiguration()
        {
            return _configurationRoot;
        }

        public void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, false)
                .AddEnvironmentVariables();

            _configurationRoot = builder.Build();
            _configurationRoot.Bind(AppSettings);
        }


    }
}