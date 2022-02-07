using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Gwenael.Web.App_Startup
{
    internal static class Logging
    {
        internal static void Configure(
            ILoggingBuilder loggingBuilder,
            IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();


            loggingBuilder.AddSerilog(Log.Logger);
        }
    }
}