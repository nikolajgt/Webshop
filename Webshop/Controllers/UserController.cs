

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Webshop.Interface;
using Webshop.Models;
using Webshop.Models.Base;
using Webshop.Models.DTO;
using Webshop.Models.JWT;

namespace Webshop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController 
    {
        private readonly IUserService _service;
        private readonly UserManager<Customer> _customerManager;
        private readonly UserManager<Admin> _adminManager;

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly Claim _user;
        private readonly IMapper _mapper;

        public UserController(UserManager<Customer> customerManager, UserManager<Admin> adminManager, IUserService service, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _customerManager = customerManager;
            _adminManager = adminManager;
            _contextAccessor = contextAccessor;
            _service = service;
            _user = contextAccessor.HttpContext.User.FindFirst(x => x.Value == ClaimTypes.Role);
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Post-User")]
        public async Task<IActionResult> PostUser([FromBody] UserDTO u)
        {
            IdentityResult result = new IdentityResult();
            switch (u.Roles)
            {
                case Roles.Customer:
                    var newCustomer = new Customer(u.UserName, u.Email, u.Firstname, u.Lastname, u.Balance, u.Roles);
                    result = await _customerManager.CreateAsync(newCustomer, u.tempPassword);
                    break;

                case Roles.Admin:
                    var newAdmin = new Admin(u.UserName, u.Email, u.Firstname, u.Lastname, u.Balance, u.Roles);
                    result = await _adminManager.CreateAsync(newAdmin, u.tempPassword);
                    break;
            }

            if (!result.Succeeded)
            {
                IdentityErrorDescriber errorDescriber = new IdentityErrorDescriber();
                IdentityError primaryError = result.Errors.FirstOrDefault();

                if (primaryError.Code == nameof(errorDescriber.DuplicateEmail))
                {
                    return new ConflictObjectResult("Email already exist");
                }
                else if (primaryError.Code == nameof(errorDescriber.DuplicateUserName))
                {
                    return new ConflictObjectResult("Username already exist");
                }
            }

            await _service.SaveAsync();
            return new OkObjectResult(true);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDTO l)
        {
            AuthenticateResponse auth = new AuthenticateResponse();

            switch(l.Roles)
            {
                case Roles.Customer:
                    var customer = await _customerManager.FindByNameAsync(l.UserName);
                    if (customer == null)
                        return new UnauthorizedObjectResult(customer);

                    bool isCustomerCorrectPassword = await _customerManager.CheckPasswordAsync(customer, l.tempPassword);
                    if (!isCustomerCorrectPassword)
                        return new UnauthorizedObjectResult(customer);

                    auth = await _service.GenerateTokens(customer, IpAddress());
                    await _service.SaveAsync();
                    return new OkObjectResult(auth);


                case Roles.Admin:
                    var admin = await _adminManager.FindByNameAsync(l.UserName);
                    if(admin == null)
                        return new UnauthorizedObjectResult(admin);

                    bool isAdminCorrectPassword = await _adminManager.CheckPasswordAsync(admin, l.tempPassword);
                    if(!isAdminCorrectPassword)
                        return new UnauthorizedObjectResult(admin);

                    auth = await _service.GenerateTokens(admin, IpAddress());
                    await _service.SaveAsync();
                    return new OkObjectResult(auth);

            }
            return new UnauthorizedObjectResult("Weird");
        }

        [HttpPost("Update-User-Information")]
        public async Task<IActionResult> UpdateUserInformation([FromBody] User user)
        {
            bool response = false;
            switch (user.Roles)
            {
                case Roles.Customer:
                    Customer customer = _mapper.Map<Customer>(user);
                    response = await _service._genericCustomerRepository.UpdateUserEntityAsync(customer);
                    if (!response)
                        return new UnauthorizedObjectResult(response);

                    await _service.SaveAsync();
                    return new OkObjectResult(response);

                case Roles.Admin:
                    Admin admin = _mapper.Map<Admin>(user);
                    response = await _service._genericAdminRepository.UpdateUserEntityAsync(admin);
                    if(!response)
                        return new UnauthorizedObjectResult(response);

                    await _service.SaveAsync();
                    return new OkObjectResult(response);
            }
            return new UnauthorizedObjectResult(response);
        }
        private string IpAddress()
        {
            if (_contextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                return _contextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
            else
                return _contextAccessor.HttpContext?.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
