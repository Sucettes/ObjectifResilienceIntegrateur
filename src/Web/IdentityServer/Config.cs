using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Gwenael.Web.IdentityServer
{
    public class Config
    {
        private const string GwenaelApiResource = "Gwenael.api";
        private const string GwenaelScopeApi = "api";

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = GwenaelApiResource,
                    DisplayName = "Gwenael Api",
                    Description = "Allow the application to access Gwenael Api on your behalf",
                    Scopes = new List<string> { GwenaelScopeApi },
                    UserClaims = new List<string> { "role" }
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope(GwenaelScopeApi),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new()
                {
                    ClientId = "Gwenael.client",
                    ClientName = "Gwenael",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequirePkce = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireClientSecret = false,
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        GwenaelScopeApi
                    },
                    AllowedCorsOrigins = new List<string>()
                    {
                        "http://localhost:3000",
                        "https://Gwenael.spektrum.media"
                    },
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    UpdateAccessTokenClaimsOnRefresh = true
                }
            };
        }
    }
}