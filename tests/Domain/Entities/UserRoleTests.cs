using Microsoft.EntityFrameworkCore;
using Shouldly;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Gwenael.Tests.Helpers;
using Gwenael.Tests.Helpers.Factories;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Domain.Entities
{
    public class UserRoleTests : TestHelperBase
    {
        public UserRoleTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact]
        public void Ctor_ShouldSetPropertiesProperly()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GwenaelDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new GwenaelDbContext(options);

            var user = UserFakerFactory.UserFaker().Generate();
            var role = RoleFakerFactory.RoleFaker().Generate();

            context.Add(user);
            context.Add(role);
            
            // Act
            var userRole = new UserRole(user.Id, role.Id);
            context.Add(userRole);
            
            // Assert
            userRole.UserId.ShouldBe(user.Id);
            userRole.User.ShouldBe(user);
            userRole.RoleId.ShouldBe(role.Id);
            userRole.Role.ShouldBe(role);
        }
    }
}
