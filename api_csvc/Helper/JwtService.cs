using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api_csvc.Helper
{
    public static class JwtService
    {
        private static readonly string SecretKey = ConfigurationManager.AppSettings["JWTSecretKey"] ?? "your-super-secret-key-that-is-at-least-32-characters-long";
        private static readonly string Issuer = ConfigurationManager.AppSettings["JWTIssuer"] ?? "api_csvc";
        private static readonly string Audience = ConfigurationManager.AppSettings["JWTAudience"] ?? "api_csvc_users";
        private static readonly TimeSpan TokenExpiry = TimeSpan.FromHours(Convert.ToDouble(ConfigurationManager.AppSettings["JWTExpiryHours"] ?? "24"));

        public static string GenerateToken(api_csvc.Models.Account user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.id_account.ToString()),
                new Claim(ClaimTypes.Name, user.name ?? user.username ?? user.email),
                new Claim(ClaimTypes.Email, user.email ?? ""),
                new Claim("id_role", user.id_role?.ToString() ?? ""),
                new Claim("username", user.username ?? "")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenExpiry),
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(SecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ValidateAudience = true,
                    ValidAudience = Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public static int? GetUserIdFromToken(string token)
        {
            var principal = ValidateToken(token);
            if (principal == null) return null;

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }

        public static int? GetUserRoleFromToken(string token)
        {
            var principal = ValidateToken(token);
            if (principal == null) return null;

            var roleClaim = principal.FindFirst("id_role");
            if (roleClaim != null && int.TryParse(roleClaim.Value, out int roleId))
            {
                return roleId;
            }
            return null;
        }

        public static string GetUsernameFromToken(string token)
        {
            var principal = ValidateToken(token);
            if (principal == null) return null;

            return principal.FindFirst("username")?.Value;
        }

        public static string GetEmailFromToken(string token)
        {
            var principal = ValidateToken(token);
            if (principal == null) return null;

            return principal.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
} 