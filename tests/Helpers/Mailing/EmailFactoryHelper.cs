using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using Gwenael.Application.Mailing;
using Gwenael.Application.Settings;
using Xunit.Abstractions;

namespace Gwenael.Tests.Helpers.Mailing
{
    public class EmailFactoryHelper : TestHelperBase
    {
        public EmailFactoryHelper(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        public static EmailFactory ArrangeEmailFactory(string fromAddress, string fromName)
        {
            var renderer = new Mock<ITemplateRenderer>();
            var sender = new Mock<ISender>();
            var mailingSettings = new Mock<IOptions<MailingSettings>>();
            mailingSettings.Setup(x => x.Value).Returns(new MailingSettings
                { FromAddress = fromAddress, FromName = fromName });

            var emailFact = new EmailFactory(renderer.Object, sender.Object, mailingSettings.Object);
            return emailFact;
        }

        public static IFluentEmail PrepareEmail()
        {
            var fromAddress = Faker.Internet.Email();
            var fromName = Faker.Person.FullName;
            var emailFact = ArrangeEmailFactory(fromAddress, fromName);
            var toAddress = Faker.Internet.Email();
            var subject = Faker.Lorem.Sentence();
            var message = Faker.Lorem.Paragraph();

            return emailFact.Prepare(toAddress, subject, message);
        }
    }
}