using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using My.BlazorWebAssembly.Dto;
using My.Components;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Principal;

namespace My.BlazorWebAssembly.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpClientFactory _httpClient;

        private ILocalStorageService _localStorage;

        private AuthenticationStateProvider _stateProvider;


        public IdentityService(ILocalStorageService localStorage, AuthenticationStateProvider stateProvider, IHttpClientFactory httpClient)
        {
            _localStorage = localStorage;
            _stateProvider = stateProvider;
            _httpClient = httpClient;
        }

        public async Task<IdentResponse<string>> Login(UserLogin userLogin) {

            IdentResponse<string> identResponse = null;
            

            var response = await _httpClient.CreateClient("My.LightAuthorizationService")
                .PostAsJsonAsync<UserLogin>("api/Account/login", userLogin);

            if (response.IsSuccessStatusCode)
            {
                identResponse = await response.Content.ReadFromJsonAsync<IdentResponse<string>>();
                await _localStorage.SetItemAsync(Constants.StorageTokenName, identResponse.Data);
            }
            else
            {
                await _localStorage.RemoveItemAsync(Constants.StorageTokenName);
            }


            await _stateProvider.GetAuthenticationStateAsync();

                               
            return identResponse;


        }

        public async Task Logout() {

            await _localStorage.RemoveItemAsync(Constants.StorageTokenName);

            await _stateProvider.GetAuthenticationStateAsync();
        }

    }
}
