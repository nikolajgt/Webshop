using Webshop.Interface;
using Webshop.Interface.Generic;
using Webshop.Models;
using Webshop.Models.Base;
using Webshop.Models.DTO;
using Webshop.Models.JWT;
using Webshop.Models.Products;
using Webshop.Repositorys;

namespace Webshop.Services
{
    public class AdminService : IAdminService
    {
        private readonly MyDbContext _context;
        public IGenericUserRepository<Admin> _genericAdminRepository { get; set; }
        public IGenericUserRepository<Customer> _genericCustomerRepository { get; set; }

        public IGenericProductRepository<Keyboard> _genericKeyboardRepository { get; set; }
        public IGenericProductRepository<Smartphone> _genericSmartphoneRepository { get; set; }
        public IGenericProductRepository<Microphone> _genericMicrophoneRepository { get; set; }
        public IGenericProductRepository<Mouse> _genericMouseRepository { get; set; }

        private ILogger _logger;
        private IJwtExtension _jwtService { get; set; }

        public AdminService(MyDbContext context, ILoggerFactory loggerFactory, IJwtExtension jwtExtension)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");
            _jwtService = jwtExtension;
            _genericAdminRepository = new GenericUserRepository<Admin>(context, _logger);
            _genericCustomerRepository = new GenericUserRepository<Customer>(context, _logger);
            _genericKeyboardRepository = new GenericProductRepository<Keyboard>(context, _logger);
            _genericSmartphoneRepository = new GenericProductRepository<Smartphone>(context, _logger);
            _genericMicrophoneRepository = new GenericProductRepository<Microphone>(context, _logger);
            _genericMouseRepository = new GenericProductRepository<Mouse>(context, _logger);
        }

        public async Task<bool> AddProduct(Product p)
        {
            switch (p.Category)
            {
                case ProductCategory.Keyboard:
                    Keyboard key = new Keyboard(p.Category, p.ProductName, p.ProductPrice, p.ProductQuantity);
                    return await _genericKeyboardRepository.PostProductEntityAsync(key);

                case ProductCategory.Smartphone:
                    Smartphone smart = new Smartphone(p.Category, p.ProductName, p.ProductPrice, p.ProductQuantity);
                    return await _genericSmartphoneRepository.PostProductEntityAsync(smart);

                case ProductCategory.Mircophone:
                    Microphone micro = new Microphone(p.Category, p.ProductName, p.ProductPrice, p.ProductQuantity);
                    return await _genericMicrophoneRepository.PostProductEntityAsync(micro);
            }

            return false;
        }

        public async Task<AllUserTypesDTO> GetAllUserTypesAsync()
        {
            var customers = await _genericCustomerRepository.GetAllUserEntitiesAsync();
            var admins = await _genericAdminRepository.GetAllUserEntitiesAsync();
            return new AllUserTypesDTO(customers, admins);
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

        public Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipaddress)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RevokeTokenAsync(string token, string ipadress)
        {
            throw new NotImplementedException();
        }
    }
}
