using Medilive.Assessment.Core.Tools.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Medilive.Assessment.Core.Abstract.Jwt
{
    public interface IJwtHelper
    {
        IJwtValidationResult ValidateToken(string password, string token);

        IJwtTokenGenerationResult GenerateToken(string password, Dictionary<string,string> claims);
    }
}
