using Suap.Common.Contracts;
using System.Text.Json.Serialization;

namespace Suap.Identity.Contracts
{
    public class TokenResponse : IResult<string>
    {
        public static TokenResponse FromSuccess(string data) => new TokenResponse { Data = data };

        public static TokenResponse FromError(string[] messages) => new TokenResponse { ErrorMessages = messages };

        public string? Data { get; set; }

        public string[] ErrorMessages { get; init; } = new string[0];

        public bool IsSuccess => !ErrorMessages.Any();
    }

}
