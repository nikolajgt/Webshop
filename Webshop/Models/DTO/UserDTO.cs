using System.Text.Json.Serialization;
using Webshop.Models.Base;
using Webshop.Models.JWT;

namespace Webshop.Models.DTO
{
    public class UserDTO
    {
        public UserDTO() { }

        public Guid Id { get; set; }
        public string UserName { get; set; }                 //Commented becuase of inheretance, could also override it
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public double Balance { get; set; }
        public Roles Roles { get; set; }
        public string tempPassword { get; set; }

        [JsonIgnore]
        public List<RefreshToken>? RefreshTokens { get; set; }

        public UserDTO(string username, string email, string firstname, string lastname, double balance, Roles roles)
        {
            UserName = username;
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
            Balance = balance;
            Roles = roles;
        }
    }
}
