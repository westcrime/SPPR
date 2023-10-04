using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;
using WEB_153502_Tolstoi.Services.ApiData;
using WEB_153502_Tolstoi.Services.GameService;

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

        public async Task<ResponseData<Game>> CreateProductAsync(Game product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Dishes");
            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Game>>(_serializerOptions);
                return data; // game;
            }
            _logger.LogError($"-----> object not created. Error{ response.StatusCode.ToString()}");
            return new ResponseData<Game>
            {
                Success = false,
                ErrorMessage = $"Объект не добавлен. Error:{ response.StatusCode.ToString() }"
            };
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? categoryNormalizedName = null, int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Games/");
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            };
            // добавить номер страницы в маршрут
            if (pageNo > 1)
            {
                urlString.Append($"page{pageNo}");
            };
            // добавить размер страницы в строку запроса
            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize));
            }
            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Game>>>();
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Game>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{ response.StatusCode.ToString()}");
            return new ResponseData<ListModel<Game>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
            };
        }

        public Task UpdateGameAsync(int id, Game game, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
