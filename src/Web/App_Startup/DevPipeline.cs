using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Gwenael.Web.Areas;

namespace Gwenael.Web.App_Startup
{
    internal static class DevPipeline
    {
        public static void Configure(IApplicationBuilder dev, IWebHostEnvironment hostingEnvironment)
        {
            dev.UseStatusCodePages();

            if (hostingEnvironment.IsDevelopment())
            {
                dev.UseDeveloperExceptionPage();
                dev.UseMigrationsEndPoint();


                var devFileProvider = new PhysicalFileProvider(Path.Combine(hostingEnvironment.WebRootPath, ".dev"));

                dev.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = devFileProvider,
                    DefaultContentType = "text/html",
                    ServeUnknownFileTypes = true
                });

                dev.UseDirectoryBrowser(
                    new DirectoryBrowserOptions
                    {
                        RequestPath = "",
                        FileProvider = devFileProvider
                    });
            }

            dev.UseRouting();
            dev.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute("dev_route", AreaNames.Dev, "{controller}/{action}");
            });
        }
    }
}