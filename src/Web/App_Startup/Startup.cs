using System;
using System.Text;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Gwenael.Application;
using Gwenael.Application.Identity;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Gwenael.Domain.Stores;
using Gwenael.Persistence;
using Gwenael.Web.Authorizations;
using Gwenael.Web.Extensions;
using Gwenael.Web.IdentityServer;

namespace Gwenael.Web.App_Startup
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDbContext<GwenaelDbContext>(options => options.UseSqlServer(
                _configuration.GetConnectionString("DefaultConnection"),
                optionsBuilder =>
                    optionsBuilder.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.FullName)));

            services.AddIdentity<User, Role>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 4;

                    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(365);
                    //options.Lockout.MaxFailedAccessAttempts = 4;
                })
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddUserManager<GwenaelUserManager>()
                .AddRoleManager<GwenaelRoleManager>()
                .AddSignInManager<SignInManager<User>>()
                .AddDefaultTokenProviders();

            // IDENTITY SERVER
            Console.WriteLine("ConnectionString: " + _configuration.GetConnectionString("DefaultConnection"));
            var identityServer = services.AddIdentityServer()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(
                        _configuration.GetConnectionString("DefaultConnection"),
                        optionsBuilder =>
                            optionsBuilder.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.FullName));
                })
                .AddAspNetIdentity<User>()
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryIdentityResources(Config.GetIdentityResources());

            services.AddDataProtection();

            if (_hostingEnvironment.IsProduction())
            {
                // TODO
                //identityServer.AddSigningCredential(Configuration.GetValue<string>("CertificatThumbprint"), StoreLocation.LocalMachine, nameType: NameType.Thumbprint);
            }
            else
            {
                identityServer.AddDeveloperSigningCredential();
            }

            var configTokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidIssuer"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateAudience = false,
            };
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.Authority = _configuration["JWT:ValidIssuer"];
                    config.RequireHttpsMetadata = false;
                    config.TokenValidationParameters = configTokenValidationParameters;
                });

            services.AddAuthorization();

            services.AddSingleton(configTokenValidationParameters); 
            services.AddScoped<RoleManager<Role>, GwenaelRoleManager>();
            services.AddScoped<UserManager<User>, GwenaelUserManager>();
            services.AddScoped<IUserConfirmation<User>, UserConfirmation>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddSwagger();

            services.AddMvcCore()
                .AddApiExplorer()
                .AddRazorPages();

            services.AddVersionedApiExplorer(config => { config.SubstituteApiVersionInUrl = true; })
                .AddApiVersioning(config => { config.AssumeDefaultVersionWhenUnspecified = false; });

            services.AddAutoMapper(typeof(Application.Profiles.Profiles));

            services.AddCors(config =>
            {
                config.AddPolicy("Development", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
                config.AddPolicy("Staging", builder =>
                {
                    builder.WithOrigins("https://Gwenael.spektrum.media")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }); 
                config.AddPolicy("Production", builder =>
                {
                    builder.WithOrigins("https://Gwenael.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            ApplicationModule.ConfigureServices(services, _configuration, _hostingEnvironment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, GwenaelDbContext dataContext)
        {
            if (!_hostingEnvironment.IsProduction())
                app.Map("/.dev", dev => DevPipeline.Configure(dev, _hostingEnvironment));

            app.Map("/api", api => ApiPipeline.Configure(api, _hostingEnvironment));

            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(razorPages =>
            {
                razorPages.MapRazorPages();
            });

            app.UseIdentityServer();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                serviceScope?.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            }

            dataContext.Database.Migrate();
        }
    }
}