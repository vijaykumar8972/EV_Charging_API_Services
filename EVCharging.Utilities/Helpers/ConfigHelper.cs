using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVCharging.Utilities.Helpers
{
    public class ConfigHelper
    {
        public readonly IConfigurationRoot AppSettings;

        public ConfigHelper()
        {
            AppSettings = GetConfigurationRoot();
        }
        public IConfigurationRoot GetConfigurationRoot()
        {
            IConfigurationRoot configuration = null;
            var basePath = System.AppContext.BaseDirectory;
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(basePath)
                .AddJsonFile($"appsettings.json");
            configuration = configurationBuilder.Build();
            return configuration;
        }
    }
}
