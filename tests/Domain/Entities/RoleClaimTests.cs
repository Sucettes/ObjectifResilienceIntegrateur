using Shouldly;
using Gwenael.Domain.Entities;
using Gwenael.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Domain.Entities
{
    public class RoleClaimTests : TestHelperBase
    {
        public RoleClaimTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Ctor_ShouldSetPropertiesProperly()
        {
            // Arrange
            var roleId = Faker.Random.Guid();
            var claimType = Faker.Lorem.Word();
            var claimValue = Faker.Lorem.Word();
            
            // Act
            var roleClaim = new RoleClaim(roleId, claimType, claimValue);
            
            // Assert
            roleClaim.RoleId.ShouldBe(roleId);
            roleClaim.ClaimType.ShouldBe(claimType);
            roleClaim.ClaimValue.ShouldBe(claimValue);
        }
        
        [Fact]
        public void ToString_ShouldReturnClaimValue()
        {
            // Arrange
            var roleClaim = new RoleClaim
            {
                ClaimValue = Faker.Lorem.Word()
            };
                
            // Act

            // Assert
            roleClaim.ToString().ShouldBe(roleClaim.ClaimValue);
        }
    }
}
