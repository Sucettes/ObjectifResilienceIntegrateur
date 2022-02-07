using Bogus.Extensions;
using Shouldly;
using Gwenael.Domain.Entities;
using Gwenael.Tests.Helpers;
using Gwenael.Tests.Helpers.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Domain.Entities
{
    public class UserTests : TestHelperBase
    {
        public UserTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Ctor_ShouldSetCreationDateToNow()
        {
            // Arrange
            
            // Act
            var user = new User();
            
            // Assert
            Assert.Equal(DateTime.UtcNow, user.CreationDate, new TimeSpan(10000));
        }
        
        [Fact]
        public void Updated_ShouldSetLastUpdateDateToNow()
        {
            // Arrange
            var user = new User();

            // Act
            Debug.Assert(user.LastUpdateDate == null, "user.LastUpdateDate == null");
            user.Updated();
            
            // Assert
            Debug.Assert(user.LastUpdateDate != null, "user.LastUpdateDate != null");
            Assert.Equal(DateTime.UtcNow, user.LastUpdateDate.Value, new TimeSpan(10000));
        }

        [Fact]
        public void Validate_ShouldReturnEmptyArray_WhenUserHasNoErrors()
        {
            // Arrange
            var user = new User
            {
                UserName = Faker.Internet.UserName(),
                FirstName = Faker.Name.FirstName(),
                LastName = Faker.Name.LastName()
            };
            
            // Act
            var errors = user.Validate();
            
            // Assert
            errors.ShouldBeEmpty();
        }
        
        [Fact]
        public void Validate_ShouldReturnASingleError_WhenUserUserNameIsNull()
        {
            // Arrange
            var user = new User
            {
                UserName = null,
                FirstName = Faker.Name.FirstName(),
                LastName = Faker.Name.LastName()
            };
            
            // Act
            var errors = user.Validate();
            
            // Assert
            errors.Length.ShouldBe(1);
            Assert.Collection(errors, 
                e => e.ShouldBe($"Required{nameof(user.UserName)}"));
        }
        
        [Fact]
        public void Validate_ShouldReturnASingleError_WhenUserFirstNameIsNull()
        {
            // Arrange
            var user = new User
            {
                UserName = Faker.Internet.UserName(),
                FirstName = null,
                LastName = Faker.Name.LastName()
            };
            
            // Act
            var errors = user.Validate();
            
            // Assert
            errors.Length.ShouldBe(1);
            Assert.Collection(errors, 
                e => e.ShouldBe($"Required{nameof(user.FirstName)}"));
        }
        
        [Fact]
        public void Validate_ShouldReturnASingleError_WhenUserLastNameIsNull()
        {
            // Arrange
            var user = new User
            {
                UserName = Faker.Internet.UserName(),
                FirstName = Faker.Name.FirstName(),
                LastName = null
            };
            
            // Act
            var errors = user.Validate();
            
            // Assert
            errors.Length.ShouldBe(1);
            Assert.Collection(errors, 
                e => e.ShouldBe($"Required{nameof(user.LastName)}"));
        }
        
        [Fact]
        public void Validate_ShouldReturnASingleError_WhenUserFirstNameExceedsMaxLength()
        {
            var maxLengthAttribute =
                GetType().GetProperty(nameof(User.FirstName))?.GetCustomAttributes(typeof(MaxLengthAttribute), false)
                    .FirstOrDefault() as MaxLengthAttribute ?? new MaxLengthAttribute(50);

            // Arrange
            var user = new User
            {
                UserName = Faker.Internet.UserName(),
                FirstName = Faker.Random.String(maxLengthAttribute?.Length + 1),
                LastName = Faker.Name.LastName() 
            };
            
            // Act
            var errors = user.Validate();
            
            // Assert
            errors.Length.ShouldBe(1);
            Assert.Collection(errors, 
                e => e.ShouldBe($"ExceededMaxLength{nameof(user.FirstName)}"));
        }
        
        [Fact]
        public void Validate_ShouldReturnASingleError_WhenUserLastNameExceedsMaxLength()
        {
            var maxLengthAttribute = GetType().GetProperty(nameof(User.LastName))?.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute ?? new MaxLengthAttribute(50);
            
            // Arrange
            var user = new User
            {
                UserName = Faker.Internet.UserName(),
                FirstName = Faker.Name.FirstName(),
                LastName = Faker.Random.String(maxLengthAttribute?.Length + 1)
            };
            
            // Act
            var errors = user.Validate();
            
            // Assert
            errors.Length.ShouldBe(1);
            Assert.Collection(errors, 
                e => e.ShouldBe($"ExceededMaxLength{nameof(user.LastName)}"));
        }
        
        [Fact]
        public void Validate_ShouldReturnMultipleErrors_WhenUserHasMultiplePropertiesWithErrors()
        {
            var firstNameMaxLengthAttribute = GetType().GetProperty(nameof(User.FirstName))?.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute ?? new MaxLengthAttribute(50);
            var lastNameMaxLengthAttribute = GetType().GetProperty(nameof(User.LastName))?.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute ?? new MaxLengthAttribute(50);
            
            // Arrange
            var user = new User
            {
                UserName = Faker.Internet.UserName().OrNull(Faker),
                FirstName = Faker.Random.String(firstNameMaxLengthAttribute?.Length + Faker.Random.Number(1)).OrNull(Faker),
                LastName = Faker.Random.String(lastNameMaxLengthAttribute?.Length + Faker.Random.Number(1)).OrNull(Faker)
            };

            var errorCount = 0;

            if (user.UserName == null)
                errorCount += 1;
            if (user.FirstName == null)
                errorCount += 1;
            else if (user.FirstName.Length > firstNameMaxLengthAttribute?.Length) 
                errorCount += 1;
            if (user.LastName == null)
                errorCount += 1;
            else if (user.LastName.Length > lastNameMaxLengthAttribute?.Length)
                errorCount += 1;

            // Act
            var errors = user.Validate();
            
            // Assert
            errors.Length.ShouldBe(errorCount);
        }
        
        [Fact]
        public void GetRoles_ShouldReturnAllRoleNamesForUser()
        {
            // Arrange
            var rolesCount = Faker.Random.Number(0, 3);
            var roles = new List<Role>();
            var userRoles = new List<UserRole>();
            for (var i = 0; i < rolesCount; i++)
            {
                var role = RoleFakerFactory.RoleFaker().Generate();
                roles.Add(role);
                userRoles.Add(new UserRole
                {
                    Role = role
                });
            }

            var user = new User()
            {
                Roles = userRoles 
            };
                
            // Act
            var roleNames = user.GetRoles();

            // Assert
            var roleNamesArray = roleNames as string[] ?? roleNames.ToArray();
            roleNamesArray.Length.ShouldBe(rolesCount);
            for (var i = 0; i < rolesCount; i++)
            {
                roleNamesArray[i].ShouldBe(roles[i].Name);
            }
        }
        
        [Fact]
        public void GetPermissions_ShouldReturnAllRoleNamesForUser()
        {
            // Arrange
            var rolesCount = Faker.Random.Number(0, 3);
            var roles = new List<Role>();
            var userRoles = new List<UserRole>();
            for (var i = 0; i < rolesCount; i++)
            {
                var role = RoleFakerFactory.RoleFaker().Generate();
                roles.Add(role);
                userRoles.Add(new UserRole
                {
                    Role = role
                });
            }

            var user = new User
            {
                Roles = userRoles 
            };
                
            // Act
            var permissionNames = user.GetPermissions();

            // Assert
            var permissionNamesArray = permissionNames as string[] ?? permissionNames.ToArray();
            permissionNamesArray.Length.ShouldBe(roles.Aggregate(0, (total, next) => total + next.GetClaims().Count()));
            for (var i = 0; i < rolesCount; i++)
            {
                foreach (var claim in roles[i].GetClaims())
                {
                    permissionNamesArray.ShouldContain(claim);
                }
            }
        }
    }
}
