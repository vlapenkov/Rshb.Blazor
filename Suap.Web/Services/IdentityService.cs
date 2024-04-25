using AntDesign;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Suap.Identity.Contracts;
using Suap.Web.Dto;
using Suap.Web.StateManagement;
using System.Net.Http.Json;
using System.Text.Json;

namespace Suap.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly IHttpClientFactory _httpClient;

        private readonly ILocalStorageService _localStorage;

        private readonly AuthenticationStateProvider _stateProvider;


        public IdentityService(ILocalStorageService localStorage, AuthenticationStateProvider stateProvider, IHttpClientFactory httpClient)
        {
            _localStorage = localStorage;
            _stateProvider = stateProvider;
            _httpClient = httpClient;
        }

        public async Task<TokenResponse> Login(UserLogin userLogin)
        {

            TokenResponse tokenResponse = null!;
            string? errorMessage = null!;

            try
            {
                HttpResponseMessage response = await _httpClient.CreateClient("Suap.IdentityService")
                    .PostAsJsonAsync("api/account/login", userLogin);

                if (response.IsSuccessStatusCode)
                {
                    tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                    await _localStorage.SetItemAsync(Constants.StorageTokenName, tokenResponse.Data);
                }
                //401 Unauthorized
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    errorMessage = $"Ошибка доступа: {response.StatusCode}, {response.Content}";

                }
                //403 Bad request
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    string sre = await response.Content.ReadAsStringAsync();

                    var error = JsonSerializer.Deserialize<ValidationError>(sre, _jsonOptions);

                    errorMessage = $"Ошибка валидации: {error.Error}";

                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    errorMessage = $"Ошибка подключения к API: {responseContent}";

                }

            }
            catch (HttpRequestException e)
            {
                errorMessage = $"Ошибка подключения к API: {e.Message}";

            }
            catch (Exception e)
            {
                errorMessage = $"Общая ошибка: {e.Message}";
            }
            finally
            {
                if (errorMessage != null)
                {
                    if (await _localStorage.ContainKeyAsync(Constants.StorageTokenName))
                    {
                        await _localStorage.RemoveItemAsync(Constants.StorageTokenName);
                    }
                    
                    
                    tokenResponse = TokenResponse.FromError([errorMessage]);
                    
                }
                await _stateProvider.GetAuthenticationStateAsync();
            }

            return tokenResponse!;


        }

        public async Task Logout()
        {

            await _localStorage.RemoveItemAsync(Constants.StorageTokenName);

            await _stateProvider.GetAuthenticationStateAsync();
        }

    }
}
