using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Gwenael.Application.Mailing;
using Gwenael.Application.Settings;
using Gwenael.Tests.Helpers;
using Gwenael.Tests.Helpers.Mailing;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Application.Mailing
{
    public class AwsSesSenderTests : TestHelperBase
    {
        public AwsSesSenderTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenSettingsIsNull()
        {
            // Arrange
            var logger = new Mock<ILogger<AwsSesSender>>();
            
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AwsSesSender(null, logger.Object));
        }
        
        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenSettingsValueIsNull()
        {
            // Arrange
            var awsSesMailingSettings = new Mock<IOptions<AwsSesMailingSettings>>();
            var logger = new Mock<ILogger<AwsSesSender>>();
            
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AwsSesSender(awsSesMailingSettings.Object, logger.Object));
        }
        
        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            // Arrange
            var awsSesMailingSettings = new Mock<IOptions<AwsSesMailingSettings>>();
            
            // Act
            
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AwsSesSender(awsSesMailingSettings.Object, null));
        }

        [Fact]
        public void Send_ShouldThrowNotImplementedException()
        {
            // Arrange
            var awsSesMailingSettings = new Mock<IOptions<AwsSesMailingSettings>>();
            awsSesMailingSettings.Setup(x => x.Value).Returns(new AwsSesMailingSettings());
            var logger = new Mock<ILogger<AwsSesSender>>();
            var email = new Mock<IFluentEmail>();
            var awsSesSender = new AwsSesSender(awsSesMailingSettings.Object, logger.Object);
            
            // Act
            
            // Assert
            Assert.Throws<NotImplementedException>(() => awsSesSender.Send(email.Object));
        }

        [Fact]
        public async Task SendAsync_ShouldReturnNull_WhenUsingCancellationToken()
        {
            // Arrange
            var awsSesMailingSettings = new Mock<IOptions<AwsSesMailingSettings>>();
            awsSesMailingSettings.Setup(x => x.Value).Returns(new AwsSesMailingSettings());
            var logger = new Mock<ILogger<AwsSesSender>>();
            var awsSesSender = new AwsSesSender(awsSesMailingSettings.Object, logger.Object);
            var cancellationToken = new CancellationTokenSource();
            
            // Act
            cancellationToken.Cancel();
            var response =
                await awsSesSender.SendAsync(EmailFactoryHelper.PrepareEmail(), cancellationToken.Token);

            // Assert
            response.ShouldBeNull();
        }
    }
}