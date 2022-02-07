using AutoMapper;
using Spk.Common.Helpers.Guard;
using Xunit;

namespace Gwenael.Tests.Application.Profiles
{
    public class ProfilesTests : IClassFixture<Gwenael.Application.Profiles.Profiles>
    {
        private readonly Gwenael.Application.Profiles.Profiles _profiles;

        public ProfilesTests(Gwenael.Application.Profiles.Profiles profiles)
        {
            _profiles = profiles.GuardIsNotNull(nameof(profiles));
        }

        [Fact]
        public void ProfilesConfig_ShouldBeValid()
        {
            // Arrange
            
            // Act
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Gwenael.Application.Profiles.Profiles());
            });
            
            // Assert
            config.AssertConfigurationIsValid();
        }
    }
}