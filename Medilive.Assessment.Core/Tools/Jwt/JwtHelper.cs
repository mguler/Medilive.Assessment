using Medilive.Assessment.Core.Abstract.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Medilive.Assessment.Core.Tools.Jwt

{
    public class JwtHelper:IJwtHelper
    {
        public JwtHelper() 
        {
        }

        public IJwtValidationResult ValidateToken(string password,string token)
        {
            var result = new JwtValidationResult { IsSuccessful = false };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var signingKeyInByteFormat = Encoding.UTF8.GetBytes(password);

                var validationParams = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Issuer",
                    ValidAudience = "Audience",
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyInByteFormat),
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParams, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                result.Claims = jwtToken.Claims.ToArray();
                result.IsSuccessful = true;
            }
            catch 
            {
                
            }
            return result;
        }

        public IJwtTokenGenerationResult GenerateToken(string password,Dictionary<string, string> claims)
        {
            var result = new JwtTokenGenerationResult();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(password);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "Issuer",
                Audience = "Audience",
                Subject = new ClaimsIdentity(claims.Keys.Select(claim => new Claim(claim, claims[claim].ToString()))),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            result.Token = tokenHandler.WriteToken(token);
            result.IsSuccessful = true;
            return result;
        }
 
    }
}
