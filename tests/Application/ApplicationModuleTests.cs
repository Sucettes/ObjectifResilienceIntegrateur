using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Gwenael.Application;
using Gwenael.Application.Services;
using Gwenael.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Application
{
    public class ApplicationModuleTests : TestHelperBase
    {
        public ApplicationModuleTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact]
        public void ConfigureServices_ShouldConfigureServicesProperly()
        {
            // Arrange
            var services = new ServiceCollection();
            var hostingEnvironment = new Mock<IWebHostEnvironment>();
            hostingEnvironment.Setup(e => e.EnvironmentName).Returns("Development");
            hostingEnvironment.Setup(e => e.WebRootPath).Returns("wwwroot");

            var configurationSectionStub = new Mock<IConfigurationSection>();
            var configurationStub = new Mock<IConfiguration>();
            configurationStub.Setup(x => x.GetSection("Mailing")).Returns(configurationSectionStub.Object);
            
            //Act
            ApplicationModule.ConfigureServices(services, configurationStub.Object, hostingEnvironment.Object);

            //Assert 
            services.Count.Should().Be(11);
            Assert.Equal(ServiceLifetime.Scoped, services.FirstOrDefault(e => e.ServiceType == typeof(IUserService))?.Lifetime);
        }
    }
}