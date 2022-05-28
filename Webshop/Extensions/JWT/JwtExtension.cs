using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Webshop.Interface;
using Webshop.Models;
using Webshop.Models.Base;
using Webshop.Models.JWT;

namespace Webshop.Extensions.JWT
{
    public class JwtExtension : IJwtExtension
    {
        private readonly string key;
        private readonly MyDbContext _context;
        private static RNGCryptoServiceProvider? _cryptoService = new RNGCryptoServiceProvider();

        public JwtExtension(IConfiguration config, MyDbContext context)
        {
            key = config.GetSection("JWTkey").ToString();
            _context = context;
        }

        public Guid? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyy = Encoding.ASCII.GetBytes(key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyy),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value);

                return userId;
            }
            catch
            {
                return null;
            }
        }

        public string GenerateJwtToken(Roles role, Guid guid, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = Encoding.ASCII.GetBytes(key);
            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, role.ToString()),
                    new Claim(ClaimTypes.Name, guid.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, username)

                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescripter);
            return tokenHandler.WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(string ipaddress)
        {
            using (var service = _cryptoService)
            {
                var randomBytes = new byte[64];
                service.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddHours(24),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipaddress,
                };
            }
        }

        public async Task<JwtSecurityToken> DecryptJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            return jsonToken as JwtSecurityToken;
        }

        public async Task<User> GetCustomerByID(Guid id)
        {
            var response = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (response == null)
                return null;

            return response;
        }
    }
}
