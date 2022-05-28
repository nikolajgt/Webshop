using Webshop.Interface.Generic;
using Webshop.Models;
using Webshop.Models.Base;
using Webshop.Models.JWT;

namespace Webshop.Interface
{
    public interface IUserService
    {
        IGenericUserRepository<Customer> _genericCustomerRepository { get; set; }
        IGenericUserRepository<Admin> _genericAdminRepository { get; set; }
        Task<AuthenticateResponse> GenerateTokens(User c, string ipaddress);
        //Sets active token to not active and gets new refresh and jwt
        Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipaddress);

        //Sets active token to not active
        Task<bool> RevokeTokenAsync(string token, string ipadress);
        Task SaveAsync();
        Task Dispose();
    }
}
