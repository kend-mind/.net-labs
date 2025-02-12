using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace VPBANK.RMD.Utils.Security.SRAJose
{
    public sealed class JwtSraTokenBuilder
    {
        private string _subject = string.Empty;
        private string _issuer = string.Empty;
        private string _audience = string.Empty;
        private int _expiryInMinutes = 20;
        private float _notBeforeInSeconds = 20;
        private List<Claim> _claims = new List<Claim>();

        public JwtSraTokenBuilder AddSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public JwtSraTokenBuilder AddIssuer(string issuer)
        {
            _issuer = issuer;
            return this;
        }

        public JwtSraTokenBuilder AddAudience(string audience)
        {
            _audience = audience;
            return this;
        }

        public JwtSraTokenBuilder AddExpiry(int expiryInMinutes)
        {
            _expiryInMinutes = expiryInMinutes;
            return this;
        }

        public JwtSraTokenBuilder AddNotBefore(float notBeforeInSeconds)
        {
            _notBeforeInSeconds = notBeforeInSeconds;
            return this;
        }

        public JwtSraTokenBuilder AddClaims(List<Claim> claims)
        {
            _claims = claims;
            return this;
        }

        public List<Claim> Build()
        {
            EnsureArguments();
            var dateUtc = DateTime.Now;
            _claims.Add(new Claim(JwtRegisteredClaimNames.Sub, _subject));
            _claims.Add(new Claim(JwtRegisteredClaimNames.Iss, _issuer));
            _claims.Add(new Claim(JwtRegisteredClaimNames.Aud, _audience));
            _claims.Add(new Claim(JwtRegisteredClaimNames.Exp, Convert.ToString(ConvertToTimestamp(dateUtc.AddMinutes(_expiryInMinutes))), ClaimValueTypes.Integer));
            _claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, Convert.ToString(ConvertToTimestamp(dateUtc.AddSeconds(-_notBeforeInSeconds).AddDays(-10))), ClaimValueTypes.Integer));
            return _claims;
        }

        private static long ConvertToTimestamp(DateTime value)
        {
            DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }

        #region " private "

        private void EnsureArguments()
        {
            if (string.IsNullOrEmpty(_subject))
                throw new ArgumentNullException($"Subject");

            if (string.IsNullOrEmpty(_issuer))
                throw new ArgumentNullException($"Issuer");

            if (string.IsNullOrEmpty(_audience))
                throw new ArgumentNullException($"Audience");

            if (_expiryInMinutes == 0)
                throw new ArgumentNullException($"Expiry In Minutes");

            if (_notBeforeInSeconds == 0)
                throw new ArgumentNullException($"Not Before In Seconds");
        }

        #endregion
    }
}
