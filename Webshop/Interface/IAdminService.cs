using Webshop.Interface.Generic;
using Webshop.Models;
using Webshop.Models.Base;
using Webshop.Models.DTO;
using Webshop.Models.JWT;

namespace Webshop.Interface
{
    public interface IAdminService
    {
        Task<bool> AddProduct(Product p);
        //IGenericUserRepository<Admin> _genericAdminRepository { get; set; }
        //IGenericUserRepository<Customer> _genericCustomerRepository { get; set; }
        Task<AllUserTypesDTO> GetAllUserTypesAsync();
        Task SaveAsync();
        Task Dispose();
    }
}
