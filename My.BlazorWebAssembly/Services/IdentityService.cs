using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using My.BlazorWebAssembly.Dto;
using My.Components;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Principal;

namespace My.BlazorWebAssembly.Services
{
    public class IdentityService : IIdentityService
    {
        private HttpClient _httpClient;

        private ILocalStorageService _localStorage;

        private AuthenticationStateProvider _stateProvider;

//        string fakeResponse = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.B-bbeuxlc4jWG8VvF_bIFEeJL1Ij5aKBeD28qedmuEA";

        public IdentityService(ILocalStorageService localStorage, AuthenticationStateProvider stateProvider, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _stateProvider = stateProvider;
            _httpClient = httpClient;
        }

        public async Task<IdentResponse<string>> Login(UserLogin userLogin) {

            IdentResponse<string> identResponse = null;

            var response = await _httpClient.PostAsJsonAsync<UserLogin>("api/Account", userLogin);

            if (response.IsSuccessStatusCode)
            {
                 identResponse = await response.Content.ReadFromJsonAsync<IdentResponse<string>>();
            }
            //time = await response.Content.ReadAsStringAsync();

            await _localStorage.SetItemAsync(Constants.StorageTokenName, identResponse.Data);

            await _stateProvider.GetAuthenticationStateAsync();

            //return await Task.FromResult( new IdentResponse<string> {Data = fakeResponse, Success = true });                      
            return identResponse;


        }

    }
}
