using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Todo.Core.Models
{
    public static class AppSettings
    {
        #region Properties

        public static String StringConnection
        {
            get
            {
                return Get("SqlServerConnection");
            }
        }

        #endregion

        #region Methods

        private static String Get(String key)
        {
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
                IConfigurationRoot root = builder.Build();
                return root.GetConnectionString(key);
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
        }

        #endregion
    }
}
