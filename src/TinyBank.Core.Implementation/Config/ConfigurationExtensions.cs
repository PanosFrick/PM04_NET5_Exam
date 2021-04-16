using Microsoft.Extensions.Configuration;

using TinyBank.Core.Config;

namespace TinyBank.Core.Implementation.Config
{
    public static class ConfigurationExtensions
    {
        public static AppConfig ReadAppConfiguration(
            this IConfiguration @this)
        {
            return new AppConfig() {
                DatabaseConnectionString = @this.GetConnectionString("tinyBank")
            };
        }
    }
}
