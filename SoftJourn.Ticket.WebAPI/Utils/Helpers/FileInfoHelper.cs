using Microsoft.Extensions.Configuration;
using System.IO;


namespace WebAPI.Utils.Helpers
{
    /// <summary>
    /// Helper for get information about reading files from the appsettings.json
    /// </summary>
    public sealed class FileInfoHelper
    {

        /// <summary>
        /// Get File's Path
        /// </summary>
        /// <returns>The string with the path </returns>
        public string GetFilePathString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var value = config.GetValue<string>("FilePath");


            return value;
        }

        /// <summary>
        /// Get File's name
        /// </summary>
        /// <value>String: File marker selected</value>
        /// <returns>The string with the file name </returns>
        public string GetFileNameString(string fileName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var value = config.GetValue<string>(fileName);


            return value;
        }
    }
}
