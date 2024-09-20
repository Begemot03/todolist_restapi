namespace API.Configuration.Provider
{
    public interface IConfigurationProvider
    {
        AppSettings AppSettings { get; }

        IConfiguration GetConfiguration();
    }
}