using Microsoft.Extensions.Configuration;

namespace Todo.Domain.Shareds;

public static class AppSettings
{
    #region Properties

    public static string StringConnection => Get("SqlServerConnection");

    #endregion

    #region Methods

    private static string Get(string key)
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
