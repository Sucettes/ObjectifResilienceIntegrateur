using System.Linq;
using FluentAssertions;
using FluentEmail.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Gwenael.Application.Mailing;
using Gwenael.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Application.Mailing
{
    public class MailingServicesTests : TestHelperBase
    {
        public MailingServicesTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact]
        public void Configure_ShouldConfigureMailingServicesProperly_WhenIsDevelopmentEnvironment()
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
            MailingServices.Configure(services, hostingEnvironment.Object, configurationStub.Object);

            //Assert 
            services.Count.Should().Be(10);
            Assert.Equal(ServiceLifetime.Singleton, services.FirstOrDefault(e => e.ServiceType == typeof(ISender))?.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, services.FirstOrDefault(e => e.ServiceType == typeof(ITemplateRenderer))?.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, services.FirstOrDefault(e => e.ServiceType == typeof(IEmailFactory))?.Lifetime);
        }
        
        [Fact]
        public void Configure_ShouldConfigureMailingServicesProperly_WhenIsProductionEnvironment()
        {
            // Arrange
            var services = new ServiceCollection();
            var hostingEnvironment = new Mock<IWebHostEnvironment>();
            hostingEnvironment.Setup(e => e.EnvironmentName).Returns("Production");
            hostingEnvironment.Setup(e => e.WebRootPath).Returns(Faker.System.DirectoryPath);

            var configurationSectionStub = new Mock<IConfigurationSection>();
            var configurationStub = new Mock<IConfiguration>();
            configurationStub.Setup(x => x.GetSection("Mailing")).Returns(configurationSectionStub.Object);
            configurationStub.Setup(x => x.GetSection("AwsSes")).Returns(configurationSectionStub.Object);
            
            //Act
            MailingServices.Configure(services, hostingEnvironment.Object, configurationStub.Object);

            //Assert 
            services.Count.Should().Be(12);
            Assert.Equal(ServiceLifetime.Singleton, services.FirstOrDefault(e => e.ServiceType == typeof(ISender))?.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, services.FirstOrDefault(e => e.ServiceType == typeof(ITemplateRenderer))?.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, services.FirstOrDefault(e => e.ServiceType == typeof(IEmailFactory))?.Lifetime);
        }
    }
}