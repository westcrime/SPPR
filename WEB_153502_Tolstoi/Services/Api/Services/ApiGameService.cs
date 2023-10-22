using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Web_153502_Tolstoi.API.Services;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;
using WEB_153502_Tolstoi.Services.ApiData;

namespace WEB_153502_Tolstoi.Services.Api.Services
{
    public class ApiGameService : IGameService
    {
        private HttpClient _httpClient;
        private string? _pageSize;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiGameService> _logger;
        private IConfiguration _configuration;

        public ApiGameService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiGameService> logger, IOptions<UriData> uriDataOptions)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(uriDataOptions.Value.ApiUri);
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<Game>> CreateGameAsync(Game game)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Games");
            var response = await _httpClient.PostAsJsonAsync(uri, game, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Game>>(_serializerOptions);
                return data; // game;
            }
            _logger.LogError($"-----> object not created. Error{response.StatusCode.ToString()}");
            return new ResponseData<Game>
            {
                Success = false,
                ErrorMessage = $"Объект не добавлен. Error:{response.StatusCode.ToString()}"
            };
        }

        public async Task DeleteGameAsync(int id)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Games/{id}")
            };
            await _httpClient.SendAsync(request);
        }

        public Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<List<Game>>> GetFullGameListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Games/");
            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var answer = await response.Content.ReadFromJsonAsync<ResponseData<List<Game>>>();
                    return answer;
                }
                catch (System.Text.Json.JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<List<Game>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode.ToString()}");
            return new ResponseData<List<Game>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
            };
        }


        public async Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? categoryNormalizedName = null, int pageNo = 1, int pageSize = 3)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Games/");
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            };
            urlString.Append($"page{pageNo}");
            // добавить размер страницы в строку запроса
            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", pageSize.ToString()));
            }
            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var answer = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Game>>>();
                    return answer;
                }
                catch (System.Text.Json.JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Game>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode.ToString()}");
            return new ResponseData<ListModel<Game>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
            };
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Games/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent =
            new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;
            await _httpClient.SendAsync(request);
            return new ResponseData<string>()
            {
                Success = true,
                Data = $"{image.FileName} is added",
                ErrorMessage = null
            };
        }

        public async Task UpdateGameAsync(int id, Game game)
        {
            var gameJson = JsonConvert.SerializeObject(game); // Сериализуем объект game в JSON

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Games/{id}"),
                Content = new StringContent(gameJson, Encoding.UTF8, "application/json") // Добавляем JSON к телу запроса

            };
            await _httpClient.SendAsync(request);
        }
    }
}
