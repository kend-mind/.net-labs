using System;
using System.Collections.Generic;
using System.Security.Claims;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography;
using System.Linq;
using VPBANK.RMD.Utils.Security.Utilities;
using Jose;
using VPBANK.RMD.Utils.Security.Models;
using Serilog;
using Newtonsoft.Json;

namespace VPBANK.RMD.Utils.Security.SRAJose
{
    public class TokenProducer
    {
        public static string Token(UserPayload jwtPayload, string privateKey, List<Claim> claims)
        {
            if (claims == null || claims.Count == 0)
                claims = new List<Claim>();
            claims.Add(new Claim(UserClaimKey.ID, Convert.ToString(jwtPayload.Id), ClaimValueTypes.Integer));
            claims.Add(new Claim(UserClaimKey.USERNAME, jwtPayload.Username));
            claims.Add(new Claim(UserClaimKey.EMAIL, jwtPayload.Email));
            claims.Add(new Claim(UserClaimKey.DEFAULT_ROLE, jwtPayload.DefaultRole));
            claims.Add(new Claim(UserClaimKey.DATA_ROLE, jwtPayload.DataRole));
            claims.Add(new Claim(UserClaimKey.FUNCTION_ROLE, jwtPayload.FunctionRole));
            claims.Add(new Claim(UserClaimKey.COMPONENT_ROLE, string.IsNullOrEmpty(jwtPayload.ComponentRole) ? "_COM" : jwtPayload.ComponentRole));
            claims.Add(new Claim(UserClaimKey.STATUS, jwtPayload.Status));
            return CreateToken(claims, privateKey);
        }

        private static string CreateToken(List<Claim> claims, string privateKey)
        {
            Log.Information($"TokenProducer->Claims: {JsonConvert.SerializeObject(claims, Formatting.Indented)}");
            Log.Information($"TokenProducer->Private_Key: {privateKey}");

            string jwt = string.Empty;
            RsaPrivateCrtKeyParameters keyPair;

            using (var sr = new System.IO.StringReader(privateKey))
            {
                PemReader pr = new PemReader(sr);
                keyPair = (RsaPrivateCrtKeyParameters)pr.ReadObject();
                //Log.Information($"Key_Pair: {JsonConvert.SerializeObject(keyPair, Formatting.Indented)}");
            }

            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(keyPair);
            Log.Information($"RSA_Params: {JsonConvert.SerializeObject(rsaParams, Formatting.Indented)}");

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                Dictionary<string, object> payload = claims.ToDictionary(k => k.Type, v => ClaimValueTypes.Integer.Equals(v.ValueType) ? (object)Convert.ToInt32(v.Value) : v.Value);
                Log.Information($"PAYLOAD: {JsonConvert.SerializeObject(payload, Formatting.Indented)}");
                jwt = JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256);
            }

            return jwt;
        }
    }
}
