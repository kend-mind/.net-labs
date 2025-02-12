using System;
using System.Security.Cryptography;
using Jose;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Parameters;
using VPBANK.RMD.Utils.Security.Models;
using VPBANK.RMD.Utils.Security.Utilities;

namespace VPBANK.RMD.Utils.Security.SRAJose
{
    public static class TokenConsumer
    {
        public static UserPayload Consume(string token, string publicKey)
        {
            var json = DecodeToken(token, publicKey);
            var jObject = (JObject)JsonConvert.DeserializeObject(json);
            UserPayload jwtPayload = new UserPayload();
            jwtPayload.Id = jObject[UserClaimKey.ID].Value<int>();
            jwtPayload.Username = jObject[UserClaimKey.USERNAME].Value<string>();
            jwtPayload.Email = jObject[UserClaimKey.EMAIL].Value<string>();
            jwtPayload.DefaultRole = jObject[UserClaimKey.DEFAULT_ROLE].Value<string>();
            jwtPayload.DataRole = jObject[UserClaimKey.DATA_ROLE].Value<string>();
            jwtPayload.FunctionRole = jObject[UserClaimKey.FUNCTION_ROLE].Value<string>();
            jwtPayload.ComponentRole = jObject[UserClaimKey.COMPONENT_ROLE].Value<string>();
            jwtPayload.Status = jObject[UserClaimKey.STATUS].Value<string>();

            return jwtPayload;
        }

        private static string DecodeToken(string token, string publicKey)
        {
            RSAParameters rsaParams;

            using (var tr = new System.IO.StringReader(publicKey))
            {
                var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(tr);
                var publicKeyParams = pemReader.ReadObject() as RsaKeyParameters;
                if (publicKeyParams == null)
                {
                    throw new Exception("Could not read RSA public key");
                }

                rsaParams = DotNetUtilities.ToRSAParameters(publicKeyParams);
            }

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                // This will throw if the signature is invalid
                return JWT.Decode(token, rsa, Jose.JwsAlgorithm.RS256);
            }
        }
    }
}
