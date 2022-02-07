using Shouldly;
using Gwenael.Domain.Entities;
using Gwenael.Tests.Helpers;
using Gwenael.Tests.Helpers.Factories;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Domain.Entities
{
    public class RoleTests : TestHelperBase
    {
        public RoleTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact]
        public void Ctor_ShouldSetPropertiesProperly()
        {
            // Arrange
            var name = Faker.Lorem.Word();
            var permissionClaims = Faker.Lorem.Words(5);
            var active = Faker.Random.Bool();
            
            // Act
            var role = new Role(name, permissionClaims, active);
            
            // Assert
            role.Name.ShouldBe(name);
            role.GetClaims().Count().ShouldBe(permissionClaims.Length);
            Assert.Collection(role.GetClaims(),
                c => c.ShouldBe(permissionClaims[0]),
                c => c.ShouldBe(permissionClaims[1]),
                c => c.ShouldBe(permissionClaims[2]),
                c => c.ShouldBe(permissionClaims[3]),
                c => c.ShouldBe(permissionClaims[4]));
            role.Active.ShouldBe(active);
        }
        
        [Fact]
        public void Ctor_ActiveShouldDefaultToTrue()
        {
            // Arrange
            
            // Act
            var role = new Role(Faker.Lorem.Word(), Faker.Lorem.Words());
            
            // Assert
            role.Active.ShouldBeTrue();
        }
        
        [Fact]
        public void GetClaims_ShouldReturnAllClaimValuesForRole()
        {
            // Arrange
            var role = RoleFakerFactory.RoleFaker().Generate();
            var claimsCount = Faker.Random.Number(0, 5);
            var roleClaims = new List<RoleClaim>();
            for (var i = 0; i < claimsCount; i++)
            {
                roleClaims.Add(new RoleClaim
                {
                    Id = Faker.Random.Int(),
                    RoleId = role.Id,
                    ClaimType = Faker.Lorem.Word(),
                    ClaimValue = Faker.Lorem.Word()
                });
            }
            role.Claims = roleClaims;
                
            // Act
            var claims = role.GetClaims().ToArray();

            // Assert
            claims.Length.ShouldBe(claimsCount);
            for (var i = 0; i < claimsCount; i++)
            {
                claims[i].ShouldBe(roleClaims[i].ClaimValue);
            }
        }
    }
}
