using System;
using System.Linq;
using FluentEmail.Core.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Gwenael.Application.Mailing;
using Gwenael.Application.Settings;
using Gwenael.Tests.Helpers;
using Gwenael.Tests.Helpers.Mailing;
using Xunit;
using Xunit.Abstractions;

namespace Gwenael.Tests.Application.Mailing
{
    public class EmailFactoryTests : TestHelperBase
    {
        public EmailFactoryTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenSettingsIsNull()
        {
            // Arrange
            var renderer = new Mock<ITemplateRenderer>();
            var sender = new Mock<ISender>();
            
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new EmailFactory(renderer.Object, sender.Object, null));
        }
        
        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenSettingsValueIsNull()
        {
            // Arrange
            var renderer = new Mock<ITemplateRenderer>();
            var sender = new Mock<ISender>();
            var mailingSettings = new Mock<IOptions<MailingSettings>>();
            
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new EmailFactory(renderer.Object, sender.Object, mailingSettings.Object));
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenFromAddressIsNull()
        {
            // Arrange
            var renderer = new Mock<ITemplateRenderer>();
            var sender = new Mock<ISender>();
            var mailingSettings = new Mock<IOptions<MailingSettings>>();
            mailingSettings.Setup(x => x.Value).Returns(new MailingSettings
                { FromAddress = null, FromName = Faker.Person.FullName });

            // Act
            
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                new EmailFactory(renderer.Object, sender.Object, mailingSettings.Object));
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenToAddressIsNull()
        {
            // Arrange
            var renderer = new Mock<ITemplateRenderer>();
            var sender = new Mock<ISender>();
            var mailingSettings = new Mock<IOptions<MailingSettings>>();
            mailingSettings.Setup(x => x.Value).Returns(new MailingSettings
                { FromAddress = Faker.Internet.Email(), FromName = null });

            // Act
            
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                new EmailFactory(renderer.Object, sender.Object, mailingSettings.Object));
        }

        [Fact]
        public void Prepare_ShouldReturnEmailInCorrectState_WhenProvidingOneToAddress()
        {
            // Arrange
            var fromAddress = Faker.Internet.Email();
            var fromName  = Faker.Person.FullName;
            var emailFact = EmailFactoryHelper.ArrangeEmailFactory(fromAddress, fromName);
            var toAddress = Faker.Internet.Email();
            var subject = Faker.Lorem.Sentence();
            var message = Faker.Lorem.Paragraph();
            var isHtml = Faker.Random.Bool();
            var bcc = Faker.Internet.Email();

            // Act
            var email = emailFact.Prepare(toAddress, subject, message, isHtml, bcc);

            // Assert
            email.Data.FromAddress.EmailAddress.ShouldBe(fromAddress);
            email.Data.FromAddress.Name.ShouldBe(fromName);
            email.Data.ToAddresses.First().EmailAddress.ShouldBe(toAddress);
            email.Data.Subject.ShouldBe(subject);
            email.Data.Body.ShouldBe(message);
        }

        [Theory]
        [InlineData(",")]
        [InlineData(";")]
        [InlineData("|")]
        public void Prepare_ShouldReturnCollectionOfRecipient_WhenProvidingMoreThanOneToAddress(string separator)
        {
            // Arrange
            var fromAddress = Faker.Internet.Email();
            var fromName  = Faker.Person.FullName;
            var emailFact = EmailFactoryHelper.ArrangeEmailFactory(fromAddress, fromName);
            var emailCount = Faker.Random.Number(2, 20);
            var emails = new string[emailCount];
            for (var i = 0; i < emailCount; i++)
            {
                emails[i] = Faker.Internet.Email();
            }

            var subject = Faker.Lorem.Sentence();
            var message = Faker.Lorem.Paragraph();

            // Act
            var email = emailFact.Prepare(string.Join(separator, emails), subject, message);

            // Assert
            email.Data.FromAddress.EmailAddress.ShouldBe(fromAddress);
            email.Data.FromAddress.Name.ShouldBe(fromName);
            email.Data.Subject.ShouldBe(subject);
            email.Data.Body.ShouldBe(message);
            email.Data.ToAddresses.Select(a => a.EmailAddress).ShouldBe(emails);
        }

        [Fact]
        public void PrepareWithTemplateModel_ShouldThrowInvalidOperationException_WhenTemplateIsMissing()
        {
            // Arrange
            var fromAddress = Faker.Internet.Email();
            var fromName  = Faker.Person.FullName;
            var toAddress = Faker.Internet.Email();
            var subject = Faker.Lorem.Sentence();
            var model = new FakeEmailMissingTemplate(subject, toAddress);
            var emailFact = EmailFactoryHelper.ArrangeEmailFactory(fromAddress, fromName);

            // Act
            
            // Assert
            Assert.Throws<InvalidOperationException>(() => emailFact.Prepare(model));
        }

        [Fact]
        public void PrepareWithTemplateModel_ShouldReturnEmailInCorrectState_WhenProvidingOneToAddress()
        {
            // Arrange
            var fromAddress = Faker.Internet.Email();
            var fromName  = Faker.Person.FullName;
            var toAddress = Faker.Internet.Email();
            var subject = Faker.Lorem.Sentence();
            
            var model = new FakeEmailModelBaseTemplate(subject, toAddress);
            var emailFact = EmailFactoryHelper.ArrangeEmailFactory(fromAddress, fromName);

            // Act
            var email = emailFact.Prepare(model);

            // Assert
            email.Data.FromAddress.EmailAddress.ShouldBe(fromAddress);
            email.Data.FromAddress.Name.ShouldBe(fromName);
            email.Data.ToAddresses.First().EmailAddress.ShouldBe(toAddress);
            email.Data.Subject.ShouldBe(subject);
            email.Data.Body.ShouldBe(null);
        }
    }
}