using System.Text.Json.Serialization;
using Webshop.Models.Base;

namespace Webshop.Models.JWT
{
    public class AuthenticateResponse
    {
        public Guid? UserID { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }

        public AuthenticateResponse() { }


       [JsonConstructor]
        public AuthenticateResponse(Guid userID, string firstname, string lastname, string jwtToken, string refreshToken)
        {
            UserID = userID;
            Firstname = firstname;
            Lastname = lastname;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }

        //From user object
        public AuthenticateResponse(User user, string jwt, string refreshtoken)
        {
            UserID = user.Id;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            JwtToken = jwt;
            RefreshToken = refreshtoken;
        }

        public AuthenticateResponse(Admin user, string jwt, string refreshtoken)
        {
            UserID = user.Id;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            JwtToken = jwt;
            RefreshToken = refreshtoken;
        }

    }
}
