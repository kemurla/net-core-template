using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.Extensions.Options;

using CAT.Core.Identity;

namespace CAT.Business.Identity
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtIssuerOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtIssuerOptions)
        {
            _jwtIssuerOptions = jwtIssuerOptions.Value;
            ThrowIfInvalidOptions(_jwtIssuerOptions);
        }

        public async Task<string> GenerateEncodedTokenAsync(string userId, string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtIssuerOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64),
            };

            var jwt = new JwtSecurityToken(
                issuer: _jwtIssuerOptions.Issuer,
                audience: _jwtIssuerOptions.Audience,
                claims: claims,
                notBefore: _jwtIssuerOptions.NotBefore,
                expires: DateTime.UtcNow.Add(_jwtIssuerOptions.ValidFor),
                signingCredentials: _jwtIssuerOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        /// <summary>
        /// Converts a date to Unix epoch.
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.");

            if (options.SigningCredentials == null)
                throw new ArgumentException(nameof(JwtIssuerOptions.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentException(nameof(JwtIssuerOptions.JtiGenerator));
        }
    }
}
