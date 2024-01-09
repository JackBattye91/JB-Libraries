using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
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
        public static IReturnCode<string> GenerateToken(IDictionary<string, string> pTokenBody, long pTokenLifespanSeconds = 3600)
        {
            IReturnCode<string> rc = new ReturnCode<string>();
            string? token = null;

            try
            {
                string? encryptionKey = Environment.GetEnvironmentVariable("JwtSecurityKey");
                string? issuer = Environment.GetEnvironmentVariable("JwtIssuer");
                string? audience = Environment.GetEnvironmentVariable("JwtAudience");

                if (encryptionKey == null || issuer == null || audience == null)
                {
                    throw new Exception("Unable to get JWT details");
                }

                if (rc.Success)
                {
                    SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey));
                    SigningCredentials signedCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

                    IList<Claim> claims = new List<Claim>();

                    foreach (var keypair in pTokenBody)
                    {
                        claims.Add(new Claim(keypair.Key, keypair.Value));
                    }

                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwt = tokenHandler.CreateJwtSecurityToken(
                        issuer,
                        audience,
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
                rc.AddError(new Error(ErrorCodes.SIGNING_TOKEN_FAILED, ex));
            }

            if (rc.Success)
            {
                rc.Data = token;
            }

            return rc;
        }
        public static IReturnCode<bool> ValidateToken(string pToken)
        {
            IReturnCode<bool> rc = new ReturnCode<bool>();

            try
            {
                string? encryptionKey = Environment.GetEnvironmentVariable("JwtSecurityKey");
                string? issuer = Environment.GetEnvironmentVariable("JwtIssuer");
                string? audience = Environment.GetEnvironmentVariable("JwtAudience");

                if (encryptionKey == null || issuer == null || audience == null)
                {
                    throw new Exception("Unable to JWT details");
                }

                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey));

                TokenValidationParameters validationParameters = new TokenValidationParameters();
                validationParameters.ValidIssuer = issuer;
                validationParameters.ValidAudience = audience;
                validationParameters.IssuerSigningKey = securityKey;
                validationParameters.ValidateLifetime = true;

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(pToken, validationParameters, out SecurityToken validToken);
            }
            catch (Exception ex)
            {
                rc.AddError(new NetworkError(ErrorCodes.VALIDATE_TOKEN_FAILED, HttpStatusCode.InternalServerError, ex));
            }

            return rc;
        }
    }
}
