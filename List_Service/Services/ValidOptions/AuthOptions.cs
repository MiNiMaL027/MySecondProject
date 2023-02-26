using List_Domain.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace List_Service.Services.ValidOptions
{
    /// <summary>
    /// This class contains settings for application authorization
    /// </summary>
    public class AuthOptions
    {
        public const string ISSUER = "TodoListServer";
        public const string AUDIENCE = "TodoListClient";
        const string KEY = "mysupersecret_secretkey!123";
        public const int LIFETIME_MINUTES = 1440;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }

        public static string GetUserToken (User user)
        {
            var identity = GetIdentity(user);
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME_MINUTES)),

            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private static ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "DefaultUser"),
                new Claim("UserId", user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }

        public class PasswordHashing // НОВИЙ ФАЙЛ 
        {
            public static string GetHashedPassword(string password)
            {
                var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterationCount,
                numBytesRequested: numBytesRequested));

                return hashed;
            }

            public static byte[] Salt = new byte[16] { 43, 12, 34, 99, 65, 1, 4, 3, 7, 54, 22, 54, 87, 74, 35, 17 }; //salt used for hashing passwords
            public const int iterationCount = 9683; //iterations for hashing passwords
            public const int numBytesRequested = 256 / 8; //used for hashing passwords
        }

        public static string GetRandomEmailConfirmationCode()
        {
            var rnd = new Random();
            char[] confirmationCodeChar = new char[8];
            for (int i = 0; i < confirmationCodeChar.Length; ++i) 
            {
                confirmationCodeChar[i] = Convert.ToString(rnd.Next(10))[0];
            }

            return new string(confirmationCodeChar);
        }     
    }
}
