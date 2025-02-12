using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using static VPBANK.RMD.API.Settings.AppSettings;

namespace VPBANK.RMD.API.Settings.Sections
{
    public static class JWTSetting
    {
        public static JwtSection GetJwtSection(IConfiguration configuration)
        {
            return new JwtSection
            {
                SecretKey = configuration["Jwt:SecretKey"],
                PrivateKey = configuration["Jwt:PrivateKey"],
                PublicKey = configuration["Jwt:PublicKey"],
                Subject = configuration["Jwt:Subject"],
                Issuer = configuration["Jwt:Issuer"],
                Issuers = string.IsNullOrEmpty(configuration["Jwt:Issuers"])
                    ? new List<string>()
                    : configuration["Jwt:Issuers"].ToString().Split(",").ToList(),
                Audience = configuration["Jwt:Audience"],
                Audiences = string.IsNullOrEmpty(configuration["Jwt:Audiences"]) 
                    ? new List<string>() 
                    : configuration["Jwt:Audiences"].ToString().Split(",").ToList(),
                Lifetime = int.Parse(configuration["Jwt:Lifetime"]),
                ExpirationInMins = int.Parse(configuration["Jwt:ExpirationInMins"]),
                NotBeforeInSeconds = float.Parse(configuration["Jwt:NotBeforeInSeconds"]),
                JwtTokenCookie = configuration["Jwt:JwtTokenCookie"]
            };
        }
    }
}
