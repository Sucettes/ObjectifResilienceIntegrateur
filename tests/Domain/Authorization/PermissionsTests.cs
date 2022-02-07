using Shouldly;
using Gwenael.Domain.Authorization;
using Gwenael.Domain.Entities;
using System.Linq;
using Xunit;

namespace Gwenael.Tests.Domain.Authorization
{
    public class PermissionsTests 
    {
        [Fact]
        public void Generate_ShouldReturnAListOfAllPermissions()
        {
            // Arrange
                
            // Act
            var permissions = Permissions.Generate().ToArray();

            // Assert
            permissions.Length.ShouldBe(9);
            Assert.Collection(permissions,
                p => p.ShouldBe($"Permissions.{nameof(User)}.Create"),
                p => p.ShouldBe($"Permissions.{nameof(User)}.View"),
                p => p.ShouldBe($"Permissions.{nameof(User)}.Edit"),
                p => p.ShouldBe($"Permissions.{nameof(User)}.Delete"),
                p => p.ShouldBe($"Permissions.{nameof(Role)}.Create"),
                p => p.ShouldBe($"Permissions.{nameof(Role)}.View"),
                p => p.ShouldBe($"Permissions.{nameof(Role)}.Edit"),
                p => p.ShouldBe($"Permissions.{nameof(Role)}.Delete"),
                p => p.ShouldBe("Permissions.View"));
        }
    }
}
