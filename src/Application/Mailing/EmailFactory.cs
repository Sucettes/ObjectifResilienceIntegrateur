using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Options;
using Spk.Common.Helpers.Guard;
using Spk.Common.Helpers.String;
using Gwenael.Application.Settings;

namespace Gwenael.Application.Mailing
{
    public class EmailFactory : IEmailFactory
    {
        private readonly ITemplateRenderer _renderer;
        private readonly ISender _sender;

        public EmailFactory(ITemplateRenderer renderer, ISender sender, IOptions<MailingSettings> settingsOptions)
        {
            _renderer = renderer ?? Email.DefaultRenderer;
            _sender = sender ?? Email.DefaultSender;

            var settings = settingsOptions.GuardIsNotNull(nameof(settingsOptions)).Value.GuardIsNotNull(nameof(settingsOptions.Value));

            FromAddress = settings.FromAddress.GuardIsNotNull(nameof(settings.FromAddress));
            FromName = settings.FromName.GuardIsNotNull(nameof(settings.FromName));
        }

        public string FromName { get; }

        public string FromAddress { get; }

        public IFluentEmail Prepare<TModel>(TModel model) where TModel : EmailModelBase
        {
            var modelType = model.GetType();
            var modelAssembly = modelType.GetTypeInfo().Assembly;
            var resourceNames = modelAssembly.GetManifestResourceNames();

            var searchName1 = $"{modelType.Name}_{CultureInfo.CurrentCulture.Name}.cshtml";
            var searchName2 = $"{modelType.Name}_{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}.cshtml";
            var searchName3 = $"{modelType.Name}.cshtml";

            var resourceName = resourceNames.FirstOrDefault(n => n.EndsWith(searchName1)) ??
                               resourceNames.FirstOrDefault(n => n.EndsWith(searchName2)) ??
                               resourceNames.FirstOrDefault(n => n.EndsWith(searchName3));

            if (resourceName.IsNullOrEmpty())
                throw new InvalidOperationException(
                    $@"Could not find any embedded that matches model name. Searched for the following names:
{searchName1}
{searchName2}
{searchName3}");

            var email = GetEmail(model.To, model.Subject);
            return email
                .UsingTemplateFromEmbedded(
                    resourceName,
                    model,
                    modelAssembly);
        }

        public IFluentEmail Prepare(
            string to,
            string subject,
            string message,
            bool isHtml = true,
            string bcc = null)
        {
            var email = GetEmail(
                to.GuardIsNotNullOrWhiteSpace(nameof(to)),
                subject.GuardIsNotNullOrWhiteSpace(nameof(subject)),
                bcc);

            return email.Body(message, isHtml);
        }

        private IFluentEmail GetEmail(string to, string subject, string bcc = null)
        {
            var toAddresses = to
                .Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => new Address(a.Trim()))
                .ToList();

            IFluentEmail email = new Email(
                _renderer,
                _sender,
                FromAddress,
                FromName);

            if (!string.IsNullOrEmpty(bcc))
                email = email.BCC(bcc);

            return email
                .To(toAddresses)
                .Subject(subject);
        }
    }
}