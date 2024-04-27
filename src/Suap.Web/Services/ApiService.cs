using Blazored.LocalStorage;
using Suap.Common.Contracts;
using Suap.Web.Dto;
using Suap.Web.StateManagement;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Suap.Web.Services;

public class ApiService : IApiService
{
    private static JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private readonly IHttpClientFactory ClientFactory;
    private readonly ILocalStorageService LocalStorage;

    public ApiService(IHttpClientFactory clientFactory, ILocalStorageService localStorage)
    {
        ClientFactory = clientFactory;
        LocalStorage = localStorage;
    }

    public async Task<Result<T>> GetResult<T>(string clientName, string uri)
    {

        try
        {
            var httpClient = ClientFactory.CreateClient(clientName);

            await SetTokenToHeader(httpClient);

            // Пример обработки ошибки  при Get
            HttpResponseMessage response = await httpClient.GetAsync(uri);


            if (response.IsSuccessStatusCode) //200 OK
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                T data = JsonSerializer.Deserialize<T>(jsonString, jsonOptions);

                return Result<T>.FromSuccess(data!);
            }
            //401 Unauthorized
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return Result<T>.FromError([$"Ошибка доступа: {response.StatusCode}, {response.Content}"]);


            }
            //403 Bad request
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                string sre = await response.Content.ReadAsStringAsync();

                var errorMessage = JsonSerializer.Deserialize<ValidationError>(sre, jsonOptions);

                return Result<T>.FromError([$"Ошибка валидации: {errorMessage.Error}"]);

            }
            else {

                string contents = await response.Content.ReadAsStringAsync();
                return Result<T>.FromError([$"Ошибка подключения к API: {contents}"]);
            }


        }
        catch (HttpRequestException e)
        {
            return Result<T>.FromError(["Ошибка подключения к API:" + e.Message]);

        }
        catch (Exception e)
        {
            return Result<T>.FromError(["Общая ошибка:" + e.Message]);
        }

    }

    private async Task SetTokenToHeader(HttpClient httpClient)
    {
        if (await LocalStorage.ContainKeyAsync(Constants.StorageTokenName))
        {
            string storedToken = await LocalStorage.GetItemAsync<string>(Constants.StorageTokenName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", storedToken);
        }
    }

    #region post            

    //// Пример обработки ошибки валидации при Post

    ////     отправляемый объект
    //     WeatherForecast wf= new() {Date = DateOnly.MinValue, Summary ="test", TemperatureC=510 };

    //// создаем JsonContent
    //     JsonContent content = JsonContent.Create(wf);

    ////    Пример обработки ошибки валидации при Post
    //      response = await httpClient.PostAsync("/api/WeatherForecast", content);
    #endregion
}
