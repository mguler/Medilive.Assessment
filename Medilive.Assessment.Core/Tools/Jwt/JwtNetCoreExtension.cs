using Microsoft.AspNetCore.Builder;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Medilive.Assessment.Core.Tools.Jwt
{
    public static class JwtNetCoreExtension
    {
        public static void UseJwt(this IApplicationBuilder application, string key, string name = "Token", TokenTransferType tokenTransferType = TokenTransferType.COOKIE)
        {
            application.Use(async (context, next) =>
            {
                var token = "";
                if (tokenTransferType == TokenTransferType.COOKIE)
                {
                    token = context.Request.Cookies[name];
                }
                else
                {
                    token = context.Request.Headers[name];
                }


                var result = new JwtHelper().ValidateToken(key, token);
                if (result.IsSuccessful) 
                {
                    var username = result.Claims.SingleOrDefault(claim => claim.Type == "Username")?.Value;
                    if (string.IsNullOrEmpty(username))
                    {
                        throw new Exception("Token is not valid");
                    }

                    var identity = new GenericIdentity(username, "JWT");
                    context.User = new GenericPrincipal(new ClaimsIdentity(identity, result.Claims), null);
                }

                await next();
            });
        }
    }
}
