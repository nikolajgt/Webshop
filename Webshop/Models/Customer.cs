using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Webshop.Interface.Generic;
using Webshop.Models.Base;
using Webshop.Models.JWT;

namespace Webshop.Models
{
    public class Customer : User, IUserEntities
    {
        public Customer() { }

        [Key]
        public override Guid Id { get; set; }
        public override string UserName { get; set; }                 //Commented becuase of inheretance, could also override it
        public override string Email { get; set; }
        public override string Firstname { get; set; }
        public override string Lastname { get; set; }
        public override double Balance { get; set; }
        public override Roles Roles { get; set; }

        [JsonIgnore]
        public override List<RefreshToken>? RefreshTokens { get; set; }

        public Customer(string username, string email, string firstname, string lastname, double balance, Roles role)
        {
            Id = Guid.NewGuid();
            UserName = username;
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
            Balance = balance;
            Roles = role;
        }

        public Customer(Guid id, string username, string email, string firstname, string lastname, double balance, Roles role)
        {
            Id = id;
            UserName = username;
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
            Balance = balance;
            Roles = role;
        }
    }
}

