using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Gwenael.Application.Services;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Gwenael.Tests.Helpers;
using Gwenael.Tests.Helpers.Factories;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Application.Services
{
    public class UserServiceTests : TestHelperBase
    {
        public UserServiceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenDbContextIsNull()
        {
            // Arrange
            
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new UserService(null));
        }
        
        [Fact]
        public void Search_ShouldReturnListOfUsersBasedOnExpression()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GwenaelDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new GwenaelDbContext(options);
            
            var users = UserFakerFactory.UserFaker().Generate(Faker.Random.Number(2, 10));
            context.Users.AddRange(users);
            context.SaveChanges();
                
            var expressions = new Expression<Func<User, bool>>[1];
            var nameLength = Faker.Random.Number(1, 5);
            expressions[0] = x => x.FirstName.Length > nameLength;

            // Act
            var usersList = new UserService(context).Search(null, Faker.Random.Bool(), expressions);
            
            // Assert
            usersList.Count().ShouldBe(users.Count(u => u.FirstName.Length > nameLength));
        }
    }
}