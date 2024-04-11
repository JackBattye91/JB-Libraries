using System.Text;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JB.Common.Utilities
{
    public static class JwtUtilities
    {
        /// <summary>
        /// Generate a JWT
        /// </summary>
        /// <param name="pTokenBody"></param>
        /// <param name="pTokenLifespanSeconds"></param>
        /// <returns></returns>
        public static IReturnCode<string> GenerateToken(IDictionary<string, string> pTokenBody, string pIssuer, string pSecurityKey, string? pAudience = null, long pTokenLifespanSeconds = 3600)
        {
            IReturnCode<string> rc = new ReturnCode<string>();
            string? token = null;

            try
            {
                if (rc.Success)
                {
                    if (string.IsNullOrEmpty(pIssuer)) {
                        rc.AddError(new Error("Invalid JWT"));
                    }

                    if (string.IsNullOrEmpty(pSecurityKey))
                    {
                        rc.AddError(new Error("Invalid JWT security key"));
                    }
                }

                if (rc.Success)
                {
                    SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(pSecurityKey));
                    SigningCredentials signedCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

                    IList<Claim> claims = new List<Claim>();

                    foreach (var keypair in pTokenBody)
                    {
                        claims.Add(new Claim(keypair.Key, keypair.Value));
                    }

                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwt = tokenHandler.CreateJwtSecurityToken(
                        pIssuer,
                        pAudience,
                        new ClaimsIdentity(claims),
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddSeconds(pTokenLifespanSeconds),
                        DateTime.UtcNow,
                        signedCreds
                    );

                    token = tokenHandler.WriteToken(jwt);
                }
            }
            catch (Exception ex)
            {
                rc.AddError(new Error(ex));
            }

            if (rc.Success)
            {
                rc.Data = token;
            }

            return rc;
        }
        public static IReturnCode<bool> ValidateToken(string pToken, string pIssuer, string pSecurityKey, string? pAudience = null)
        {
            IReturnCode<bool> rc = new ReturnCode<bool>();

            try
            {

                if (rc.Success)
                {
                    if (string.IsNullOrEmpty(pIssuer))
                    {
                        rc.AddError(new Error("Invalid JWT Issuer"));
                    }

                    if (string.IsNullOrEmpty(pSecurityKey))
                    {
                        rc.AddError(new Error("Invalid JWT security key"));
                    }
                }

                if (rc.Success)
                {
                    SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(pSecurityKey));

                    TokenValidationParameters validationParameters = new TokenValidationParameters();
                    validationParameters.ValidIssuer = pIssuer;
                    validationParameters.IssuerSigningKey = securityKey;
                    validationParameters.ValidateLifetime = true;
                    validationParameters.ValidateIssuer = true;

                    if (!string.IsNullOrEmpty(pAudience))
                    {
                        validationParameters.ValidateAudience = true;
                        validationParameters.ValidAudience = pAudience;
                    }

                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    tokenHandler.ValidateToken(pToken, validationParameters, out SecurityToken validToken);
                }
            }
            catch (Exception ex)
            {
                rc.AddError(new NetworkError(HttpStatusCode.InternalServerError, ex));
            }

            return rc;
        }
    }
}
