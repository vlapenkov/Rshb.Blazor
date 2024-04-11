using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace My.Auth
{
    public static class AuthJwtTokenOptions
    {
        public const string Issuer = "SomeIssuesName";

        public const string Audience = "https://awesome-website.com/";

        public const string Key = "this is my custom Secret key for authentication";

        public static SecurityKey GetSecurityKey() =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
