using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace Suap.Web.StateManagement
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ILocalStorageService _localStorage;
        private readonly ILogger<CustomAuthStateProvider> _logger;
        private readonly AppState _appState;
        private const char _roleSeparator = ',';
        private const string _authenticationType = "jwt";

        public CustomAuthStateProvider(ILogger<CustomAuthStateProvider> logger, ILocalStorageService localStorage, AppState appState)
        {

            _logger = logger;
            _localStorage = localStorage;
            _appState = appState;

        }

        public static ClaimsPrincipal Anonymous
    => new ClaimsPrincipal(new ClaimsIdentity(Array.Empty<Claim>(), string.Empty));


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new();

            var identityToken = await _localStorage.GetItemAsStringAsync(Constants.StorageTokenName);

            if (identityToken != null)
            {

                try
                {
                    identity = new ClaimsIdentity(ParseClaims(identityToken), _authenticationType);

                    ParseRoleClaims(identity);
                }
                catch
                {
                    if (await _localStorage.ContainKeyAsync(Constants.StorageTokenName))

                        await _localStorage.RemoveItemAsync(Constants.StorageTokenName);

                }
                



            }



            var user = new ClaimsPrincipal(identity);

            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            // Нотификация связанных с appState по событию
            _appState.Username = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;


            return state;


        }

        private static void ParseRoleClaims(ClaimsIdentity identity)
        {
            if (identity.Claims != null)
            {
                string[] roleClaims;
                var roleClaimJwt = identity.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value;

                if (roleClaimJwt != null && roleClaimJwt.Length > 0)
                {
                    if (roleClaimJwt.Contains(_roleSeparator))
                    {
                        roleClaims = JsonSerializer.Deserialize<string[]>(roleClaimJwt!);
                    }
                    else
                    {
                        roleClaims = [roleClaimJwt!];
                    }

                    identity.AddClaims(roleClaims?.Select(roleClaim => new Claim(ClaimTypes.Role, roleClaim)));
                }

            }
        }

        private static IEnumerable<Claim> ParseClaims(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            // We need this claim to fill AuthState.User.Identity.Name (to display current user name)
            // keyValuePairs.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Name);

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            return Convert.FromBase64String(base64);
        }

        #region obsolete code
        public Task<AuthenticationState> GetAuthenticationStateAsyncOld()
        {
            _logger.LogInformation("GetAuthenticationStateAsync called");



            ClaimsIdentity identity = new();

            //if (_appState.IsAuthenticated)
            if (true)
            {
                _logger.LogInformation("GetAuthenticationStateAsync authenticated");

                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "Vladimir"),
                }, "Custom Authentication");

                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                identity.AddClaim(new Claim(ClaimTypes.Role, "Viewer"));


                var user = new ClaimsPrincipal(identity);

                return Task.FromResult(new AuthenticationState(user));
            }


            return Task.FromResult(new AuthenticationState(Anonymous));


        }

        #endregion
    }
}
