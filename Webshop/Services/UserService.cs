using Webshop.Interface;
using Webshop.Interface.Generic;
using Webshop.Models;
using Webshop.Models.Base;
using Webshop.Models.JWT;
using Webshop.Repositorys;

namespace Webshop.Services
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _context;
        public IGenericUserRepository<Customer> _genericCustomerRepository { get; set; }
        public IGenericUserRepository<Admin> _genericAdminRepository { get; set; }

        private ILogger _logger;
        private IJwtExtension _jwtService;

        public UserService(MyDbContext context, ILoggerFactory loggerFactory, IJwtExtension jwtExtension)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logss");
            _jwtService = jwtExtension;
            _genericCustomerRepository = new GenericUserRepository<Customer>(context, _logger);
            _genericAdminRepository = new GenericUserRepository<Admin>(context, _logger);
        }

        public async Task<AuthenticateResponse> GenerateTokens(User u, string ipaddress)
        {
            var jwtToken = _jwtService.GenerateJwtToken(u.Roles, u.Id, u.UserName);
            var refreshToken = _jwtService.GenerateRefreshToken(ipaddress);
            u.RefreshTokens.Add(refreshToken);

            // either customer or admin repo
            //await _genericCustomerRepository.UpdateUserEntityAsync(u);

            return new AuthenticateResponse(u, jwtToken, refreshToken.Token);
        }

        public async Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipaddress)
        {
            var response = await _genericCustomerRepository.RefreshTokenUserEntity(token);

            if (response == null)
                return null;

            var refreshToken = response.RefreshTokens.SingleOrDefault(x => x.Token == token);
            if (!refreshToken.IsActive)
                return null;

            var newRefreshToken = _jwtService.GenerateRefreshToken(ipaddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipaddress;
            refreshToken.ReplaceByToken = newRefreshToken.Token;

            response.RefreshTokens.Add(refreshToken);

            await _genericCustomerRepository.UpdateUserEntityAsync(response);
            var jwt = _jwtService.GenerateJwtToken(response.Roles, response.Id, response.UserName);

            return new AuthenticateResponse(response, jwt, newRefreshToken.Token);
        }

        public async Task<bool> RevokeTokenAsync(string token, string ipadress)
        {
            var response = await _genericCustomerRepository.RefreshTokenUserEntity(token);
            if (response == null)
                return false;

            var refreshToken = response.RefreshTokens.SingleOrDefault(x => x.Token == token);
            if (!refreshToken.IsActive)
                return false;

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipadress;

            response.RefreshTokens?.Add(refreshToken);

            return await _genericCustomerRepository.UpdateUserEntityAsync(response);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
