using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Text.Json;

namespace My.BlazorWebAssembly.Services;

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


            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                T data = JsonSerializer.Deserialize<T>(jsonString, jsonOptions);

                return new Result<T>(data!);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return new Result<T>([$"Ошибка доступа: {response.StatusCode}, {response.Content}"]);


            }
            else
            {
                string sre = await response.Content.ReadAsStringAsync();

                var errorMessage = JsonSerializer.Deserialize<ValidationError>(sre, jsonOptions);

                return new Result<T>([$"Ошибка валидации: {errorMessage.ToString()}"]);

            }


        }
        catch (HttpRequestException e)
        {
            return new Result<T>(["Ошибка подключения к API:" + e.Message]);

        }
        catch (Exception e)
        {
            return new Result<T>(["Общая ошибка:" + e.Message]);
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
