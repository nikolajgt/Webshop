using System.IdentityModel.Tokens.Jwt;
using Webshop.Models.Base;
using Webshop.Models.JWT;

namespace Webshop.Interface
{
    public interface IJwtExtension
    {
        Guid? ValidateToken(string token);
        string GenerateJwtToken(Roles role, Guid userid, string username);

        RefreshToken GenerateRefreshToken(string ipaddress);
        Task<JwtSecurityToken> DecryptJwtToken(string token);
        Task<User> GetCustomerByID(Guid id);
    }
}
