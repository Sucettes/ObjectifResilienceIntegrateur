using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Gwenael.Domain.Entities;
using Gwenael.Domain.Stores;
using Gwenael.Tests.Helpers;
using Gwenael.Tests.Helpers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Gwenael.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Domain.Stores
{
    public class RoleManagerTests : TestHelperBase
    {
        private readonly GwenaelDbContext _context;
        private readonly GwenaelRoleManager _roleManager;
        
        public RoleManagerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            
            var options = new DbContextOptionsBuilder<GwenaelDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new GwenaelDbContext(options);
            
            var roleStore = new RoleStore(_context);
            var roleValidators = new List<Mock<IRoleValidator<Role>>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new Mock<IdentityErrorDescriber>();
            var logger = new Mock<ILogger<RoleManager<Role>>>();
            _roleManager = new GwenaelRoleManager(roleStore, roleValidators.Select(r => r.Object),
                keyNormalizer.Object, errors.Object, logger.Object);
        }
        
        [Fact]
        public async Task FindByIdAsync_ShouldReturnNull_WhenRoleIsNotFound()
        {
            // Arrange
            
            // Act
            var roleFoundById = await _roleManager.FindByIdAsync(Guid.NewGuid());

            // Assert
            roleFoundById.ShouldBeNull();
        }
        
        [Fact]
        public async Task FindByIdAsync_ShouldReturnRole_WhenRoleIsFound()
        {
            // Arrange
            var role = RoleFakerFactory.RoleFaker().Generate();

            _context.Add(role);
            await _context.SaveChangesAsync();

            // Act
            var roleFoundById = await _roleManager.FindByIdAsync(role.Id);

            // Assert
            roleFoundById.ShouldBe(role);
        }
        
        [Fact]
        public async Task FindByNameAsync_ShouldReturnNull_WhenRoleIsNotFound()
        {
            // Arrange
            
            // Act
            var roleFoundById = await _roleManager.FindByNameAsync(Faker.Random.Word());

            // Assert
            roleFoundById.ShouldBeNull();
        }
        
        [Fact]
        public async Task FindByNameAsync_ShouldReturnRole_WhenRoleIsFound()
        {
            // Arrange
            var role = RoleFakerFactory.RoleFaker().Generate();

            _context.Add(role);
            await _context.SaveChangesAsync();

            // Act
            var roleFoundById = await _roleManager.FindByNameAsync(role.Name);

            // Assert
            roleFoundById.ShouldBe(role);
        }
    }
}