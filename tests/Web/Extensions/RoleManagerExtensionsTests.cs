using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Gwenael.Domain;
using Gwenael.Domain.Entities;
using Gwenael.Tests.Helpers;
using Gwenael.Tests.Helpers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gwenael.Domain.Stores;
using Gwenael.Web.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Web.Extensions
{
    public class RoleManagerExtensionsTests : TestHelperBase
    {
        private readonly GwenaelDbContext _context;
        private readonly GwenaelRoleManager _roleManager;
        
        public RoleManagerExtensionsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
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
        public async Task SeedClaimsForSuperAdmin_ShouldAddAllClaimsForSuperAdmin()
        {
            // Arrange
            var role = RoleFakerFactory.RoleFaker().Generate();
            role.Name = Constants.Authorization.Roles.SUPER_ADMIN;
            role.Claims = new List<RoleClaim>();

            _context.Add(role);
            await _context.SaveChangesAsync();
            
            // Act
            await _roleManager.SeedClaimsForSuperAdmin();
            
            // Assert
            role.Claims.Count().ShouldBe(9);
            Assert.Collection(role.Claims,
                c => c.ClaimValue.ShouldBe($"Permissions.{nameof(User)}.Create"),
                c => c.ClaimValue.ShouldBe($"Permissions.{nameof(User)}.View"),
                c => c.ClaimValue.ShouldBe($"Permissions.{nameof(User)}.Edit"),
                c => c.ClaimValue.ShouldBe($"Permissions.{nameof(User)}.Delete"),
                c => c.ClaimValue.ShouldBe($"Permissions.{nameof(Role)}.Create"),
                c => c.ClaimValue.ShouldBe($"Permissions.{nameof(Role)}.View"),
                c => c.ClaimValue.ShouldBe($"Permissions.{nameof(Role)}.Edit"),
                c => c.ClaimValue.ShouldBe($"Permissions.{nameof(Role)}.Delete"),
                c => c.ClaimValue.ShouldBe("Permissions.View"));
        }

        [Fact]
        public async Task AddPermissionClaim_ShouldNotAddPermission_WhenRoleAlreadyHasPermission()
        {
            // Arrange
            var role = RoleFakerFactory.RoleFaker().Generate();

            _context.Add(role);
            await _context.SaveChangesAsync();
            
            // Act
            await _roleManager.AddNewPermissionClaim(role, new List<string> {role.Claims.First().ClaimValue});
            
            // Assert
            role.Claims.Count().ShouldBe(1);
            Assert.Collection(role.Claims,
                c => c.ClaimValue.ShouldBe(role.Claims.First().ClaimValue));
        }
        
        [Fact]
        public async Task AddPermissionClaim_ShouldAddPermission_WhenRoleAlreadyHasClaimValueButNotAsPermission()
        {
            // Arrange
            var role = RoleFakerFactory.RoleFaker().Generate();
            role.Claims.First().ClaimType = "Rejection";

            _context.Add(role);
            await _context.SaveChangesAsync();
            
            // Act
            await _roleManager.AddNewPermissionClaim(role, new List<string> {role.Claims.First().ClaimValue});
            
            // Assert
            role.Claims.Count().ShouldBe(2);
            Assert.Collection(role.Claims,
                c => c.ClaimValue.ShouldBe(role.Claims.ToArray()[0].ClaimValue),
                c => c.ClaimValue.ShouldBe(role.Claims.ToArray()[1].ClaimValue));
        }
        
        [Fact]
        public async Task AddPermissionClaim_ShouldAddPermission_WhenRoleIsMissingPermission()
        {
            // Arrange
            var role = RoleFakerFactory.RoleFaker().Generate();

            _context.Add(role);
            await _context.SaveChangesAsync();

            var permission = Faker.Random.String();
            
            // Act
            await _roleManager.AddNewPermissionClaim(role, new List<string> {permission});
            
            // Assert
            role.Claims.Count().ShouldBe(2);
            Assert.Collection(role.Claims,
                c => c.ClaimValue.ShouldBe(role.Claims.First().ClaimValue),
                c => c.ClaimValue.ShouldBe(permission));
        }
        
        [Fact]
        public async Task RemovePermissionClaim_ShouldNotRemovePermission_WhenPassingPermissionToMethod()
        {
            // Arrange
            var role = RoleFakerFactory.RoleFaker().Generate();

            _context.Add(role);
            await _context.SaveChangesAsync();
            
            // Act
            await _roleManager.RemoveDeprecatedPermissionClaim(role, new List<string> {role.Claims.First().ClaimValue});
            
            // Assert
            role.Claims.Count().ShouldBe(1);
            Assert.Collection(role.Claims,
                c => c.ClaimValue.ShouldBe(role.Claims.First().ClaimValue));
        }
        
        [Fact]
        public async Task RemovePermissionClaim_ShouldRemovePermission_WhenNotPassingPermissionToMethod()
        {
            // Arrange
            var role = RoleFakerFactory.RoleFaker().Generate();

            _context.Add(role);
            await _context.SaveChangesAsync();
            
            // Act
            await _roleManager.RemoveDeprecatedPermissionClaim(role, new List<string> {});
            
            // Assert
            role.Claims.ShouldBeEmpty();
        }
        
        [Fact]
        public async Task RemovePermissionClaim_ShouldNotAddPermission_WhenPermissionIsMissing()
        {
            // Arrange
            var role = RoleFakerFactory.RoleFaker().Generate();

            _context.Add(role);
            await _context.SaveChangesAsync();

            var permission = Faker.Random.String();
            
            // Act
            await _roleManager.RemoveDeprecatedPermissionClaim(role, new List<string> {role.Claims.First().ClaimValue, permission});
            
            // Assert
            role.Claims.Count().ShouldBe(1);
            Assert.Collection(role.Claims,
                c => c.ClaimValue.ShouldBe(role.Claims.First().ClaimValue));
        }
        
        [Fact]
        public async Task SyncPermissionClaim_ShouldSyncPermissionsProperly()
        {
            // Arrange
            var role = RoleFakerFactory.RoleFaker().Generate();

            _context.Add(role);
            await _context.SaveChangesAsync();

            var permission = Faker.Random.String();
            
            // Act
            await _roleManager.SyncPermissionClaim(role, new List<string> {permission});
            
            // Assert
            role.Claims.Count().ShouldBe(1);
            Assert.Collection(role.Claims,
                c => c.ClaimValue.ShouldBe(permission));
        }
    }
}