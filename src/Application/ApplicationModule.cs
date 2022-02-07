using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gwenael.Application.Mailing;
using Gwenael.Application.Services;

namespace Gwenael.Application
{
    public static class ApplicationModule
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            MailingServices.Configure(services, environment, configuration.GetSection("Mailing"));
            services.AddScoped<IUserService, UserService>(); 
        }
    }
}