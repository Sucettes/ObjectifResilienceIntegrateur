using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spk.Common.Helpers.Guard;
using Spk.Common.Helpers.String;
using Gwenael.Application.Settings;

namespace Gwenael.Application.Mailing
{
    public class AwsSesSender : ISender
    {
        private const string EmailCancelled = "Email not sent. Operation was cancelled by cancellation token.";

        private readonly ILogger<AwsSesSender> _logger;
        private readonly AmazonSimpleEmailServiceClient _client;

        public AwsSesSender(
            IOptions<AwsSesMailingSettings> settingsOptions,
            ILogger<AwsSesSender> logger)
        {
            _logger = logger.GuardIsNotNull(nameof(logger));
            var settings = settingsOptions.GuardIsNotNull(nameof(settingsOptions)).Value.GuardIsNotNull(nameof(settingsOptions.Value));

            if (settings == null) return;

            var credentials = new BasicAWSCredentials(
                settings.AccessKeyId,
                settings.AccessKeySecret);

            var regionEndpoint = settings.RegionEndpoint.IsNullOrWhiteSpace()
                ? RegionEndpoint.USEast1
                : RegionEndpoint.GetBySystemName(settings.RegionEndpoint);

            _client = new AmazonSimpleEmailServiceClient(credentials, regionEndpoint);
        }

        public SendResponse Send(IFluentEmail email, CancellationToken? token = default)
        {
            throw new NotImplementedException("Must use SendAsync");
        }

        public async Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = default)
        {
            var response = new SendResponse();
            var emailData = email.Data;

            var from = $"{emailData.FromAddress.Name} <{emailData.FromAddress.EmailAddress}>";
            var tos = emailData.ToAddresses.Select(to => to.EmailAddress).ToList();
            var bccs = emailData.BccAddresses.Select(bcc => bcc.EmailAddress).ToList();
            var subject = emailData.Subject;
            var plainTextBody = email.Data.IsHtml ? null : email.Data.Body;
            var htmlBody = email.Data.IsHtml ? email.Data.Body : null;

            var msg = new Message(new Content(subject), new Body
            {
                Text = new Content(plainTextBody),
                Html = new Content(htmlBody)
            });

            var request = new SendEmailRequest(
                from,
                new Destination(tos) { BccAddresses = bccs },
                msg);

            if (token?.IsCancellationRequested ?? false)
            {
                response.ErrorMessages.Add(EmailCancelled);
                _logger.LogWarning(EmailCancelled);
                return null;
            }

            SendEmailResponse sendEmailResponse;
            if (token.HasValue)
            {
                sendEmailResponse = await _client.SendEmailAsync(request, token.Value);
            }
            else
            {
                sendEmailResponse = await _client.SendEmailAsync(request);
            }

            if (sendEmailResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"Email sent with message id {sendEmailResponse.MessageId}");
                return response;
            }

            if ((int)sendEmailResponse.HttpStatusCode >= 400)
                response.ErrorMessages.Add(
                    $"Failed to send Email : Status code -> {sendEmailResponse.HttpStatusCode}, Data -> {sendEmailResponse.ResponseMetadata}");

            return response;
        }
    }
}