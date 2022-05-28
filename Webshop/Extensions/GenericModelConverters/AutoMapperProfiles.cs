using AutoMapper;
using Webshop.Models;
using Webshop.Models.Base;

namespace Backend_webshop.Extensions.GenericModelConverters
{
    public class UserProfileMapper : Profile
    {
        public UserProfileMapper()
        {
            CreateMap<User, Customer>();
            CreateMap<User, Admin>();
        }
    }


}
