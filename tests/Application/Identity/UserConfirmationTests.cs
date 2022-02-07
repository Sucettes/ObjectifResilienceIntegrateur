using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Gwenael.Application.Identity;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Gwenael.Tests.Helpers;
using Gwenael.Tests.Helpers.Factories;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Application.Identity
{
    public class UserConfirmationTests : TestHelperBase
    {
        public UserConfirmationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task IsConfirmedAsync_ShouldReturnUserActiveStatus(bool isActive)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GwenaelDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var context = new GwenaelDbContext(options);

            var user = UserFakerFactory.UserFaker().Generate();
            user.Active = isActive;
            
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var store = new Mock<IUserStore<User>>();
            var mgr = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            var userConfirmation = new UserConfirmation();
                
            // Act
            var isConfirmed = await userConfirmation.IsConfirmedAsync(mgr.Object, user);
            
            // Assert
            Assert.Equal(isActive, isConfirmed);
        }
    }
}