using Shouldly;
using Gwenael.Domain.Authorization;
using System;
using System.Linq;
using Gwenael.Domain;
using Xunit;

namespace Gwenael.Tests.Domain.Authorization
{
    public class SuperAdminsTests
    {
        [Fact]
        public void GetAllSuperAdmins_ShouldReturnAListOfAllSuperAdmins()
        {
            // Arrange
                
            // Act
            var superAdmins = SuperAdmins.GetAllSuperAdmins().ToArray();

            // Assert
            superAdmins.Length.ShouldBe(1);
            Assert.Collection(superAdmins,
                sa =>
                {
                    sa.Id.ShouldNotBe(Guid.Empty);
                    sa.UserName.ShouldBe("geeks");
                    sa.Email.ShouldBe(Constants.Authorization.SuperAdmins.GEEKS_AT_SEPKTRUMMEDIA);
                    sa.EmailConfirmed.ShouldBeTrue();
                    sa.TwoFactorEnabled.ShouldBeFalse();
                    sa.LockoutEnabled.ShouldBeFalse();
                    sa.Active.ShouldBeTrue();
                    sa.FirstName.ShouldBe("Geeks");
                    sa.LastName.ShouldBe("Spektrum");
                    sa.PhoneNumber.ShouldBe("4188007440");
                    sa.PhoneNumberConfirmed.ShouldBeTrue();
                });
        }
    }
}
