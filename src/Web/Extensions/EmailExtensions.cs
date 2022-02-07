using Gwenael.Application.Mailing;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Gwenael.Web.Extensions
{
    public static class EmailFactoryExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailFactory emailFactory, string email, string link)
        {
            return emailFactory
                .Prepare(email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking here</a>.")
                .SendAsync();
        }

        public static Task SendResetPasswordAsync(this IEmailFactory emailFactory, string email, string userName, string callbackUrl, string bcc = null)
        {
            return emailFactory
                .Prepare(email, "Reset Password", $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a><br/>Nom d'utilisateur: {userName}.", bcc: bcc)
                .SendAsync();
        }
    }
}