using Shouldly;
using Gwenael.Domain.Authorization;
using System;
using System.Linq;
using Gwenael.Domain;
using Xunit;

namespace Gwenael.Tests.Domain.Authorization
{
    public class RolesTests
    {
        [Fact]
        public void GetAllRoles_ShouldReturnAListOfAllRoles()
        {
            // Arrange
                
            // Act
            var roles = Roles.GetAllRoles().ToArray();

            // Assert
            roles.Length.ShouldBe(3);
            Assert.All(roles, role => role.Id.ShouldNotBe(Guid.Empty));
            Assert.Collection(roles,
                r => r.Name.ShouldBe(Constants.Authorization.Roles.SUPER_ADMIN),
                r => r.Name.ShouldBe(Constants.Authorization.Roles.ADMIN),
                r => r.Name.ShouldBe(Constants.Authorization.Roles.USER));
        }
    }
}
