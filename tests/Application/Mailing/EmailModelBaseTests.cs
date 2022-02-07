using System;
using Shouldly;
using Gwenael.Tests.Helpers;
using Gwenael.Tests.Helpers.Mailing;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Application.Mailing
{
    public class EmailModelBaseTests : TestHelperBase
    {
        public EmailModelBaseTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact]
        public void Ctor_ShouldSetPropertiesCorrectly_WhenAllArgValid()
        {
            // Arrange
            var subject = Faker.Lorem.Sentence();
            var toAddress = Faker.Internet.Email();

            // Act
            var emailModelBase = new FakeEmailModelBaseTemplate(subject, toAddress);

            //assert
            emailModelBase.Subject.ShouldBe(subject);
            emailModelBase.To.ShouldBe(toAddress);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_ShouldThrowArgumentNullException_WhenSubjectIsNullOrWhitespace(string subject)
        {
            // Arrange
            var toAddress = Faker.Internet.Email();

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new FakeEmailModelBaseTemplate(subject, toAddress));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_ShouldThrowArgumentNullException_WhenToAddressIsNullOrWhitespace(string toAddress)
        {
            // Arrange
            var subject = Faker.Lorem.Sentence();

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new FakeEmailModelBaseTemplate(subject, toAddress));
        }
    }
}