using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Gwenael.Web.Areas;

namespace Gwenael.Web.App_Startup
{
    internal static class ApiPipeline
    {
        public static void Configure(IApplicationBuilder api, IWebHostEnvironment hostingEnvironment)
        {
            api.UseRouting();
            api.UseCors(hostingEnvironment.EnvironmentName);
            api.UseAuthentication();
            api.UseAuthorization();
            api.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute("api_route", AreaNames.Api, "{controller=Home}/{action=Index}");
            });

            api.UseSwagger();
            api.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"v1/swagger.json", "Gwenael API V1");
                options.SwaggerEndpoint($"v2/swagger.json", "Gwenael API V2");
            });
        }
    }
}