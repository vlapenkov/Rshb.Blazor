using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Suap.Common.Jwt
{
    public static class AuthJwtTokenOptions
    {
        // TODO: брать из configmap
        public const string Issuer = "http://localhost:8080/realms/suap-realm";

        // TODO: брать из configmap
        public const string Audience = "http://localhost:8080/realms/suap-realm";

        // TODO: брать из Secret
        public const string Key = "this_is_my_custom Secret_key_for_authentication";

        public static SecurityKey GetSecurityKey() =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
