using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Webshop.Models.JWT;

namespace Webshop.Models.Base
{
    public class User : IdentityUser<Guid>
    {
        public User() { }

        public override Guid Id { get; set; }
        public override string UserName { get; set; }                 //Commented becuase of inheretance, could also override it
        public override string Email { get; set; }

        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual double Balance { get; set; }
        public virtual Roles Roles { get; set; }

        [JsonIgnore]
        public virtual List<RefreshToken>? RefreshTokens { get; set; }

        public User(string username, string email, string firstname, string lastname, double balance, Roles roles)
        {
            Id = Guid.NewGuid();
            UserName = username;
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
            Balance = balance;
            Roles = roles;
        }
    }
}
