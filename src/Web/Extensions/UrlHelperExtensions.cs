using System;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Gwenael.Web.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string GetLocalUrl(this IUrlHelper urlHelper, string localUrl)
        {
            return urlHelper.IsLocalUrl(localUrl) ? localUrl : urlHelper.Page("/Index");
        }
        
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, Guid userId, string code, string scheme)
        {
            return urlHelper.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId, code },
                protocol: scheme);
        }
        
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, Guid userId, string code,
            string scheme, string host = "")
        {
            if (string.IsNullOrEmpty(host))
            {
                return urlHelper.Page(
                    "/resetPassword",
                    pageHandler: null,
                    values: new { userId, code },
                    protocol: scheme);
            }

            return $"{host}reset-password?id={userId}&code={HttpUtility.UrlEncode(code)}";
        }
    }
}