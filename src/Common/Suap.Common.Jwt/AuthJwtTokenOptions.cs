using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Suap.Common.Jwt
{
    public static class AuthJwtTokenOptions
    {
        // TODO: брать из configmap
        public const string Issuer = "Suap";

        // TODO: брать из configmap
        public const string Audience = "https://replace-with-my-url.ru/";

        // TODO: брать из Secret
        public const string Key = "this_is_my_custom Secret_key_for_authentication";

        public static SecurityKey GetSecurityKey() =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
