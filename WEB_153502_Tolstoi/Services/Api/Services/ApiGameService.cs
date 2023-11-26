using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Web_153502_Tolstoi.API.Services;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;
using WEB_153502_Tolstoi.Services.ApiData;

namespace WEB_153502_Tolstoi.Services.Api.Services
{
    [Authorize]
    public class ApiGameService : IGameService
    {
        private HttpClient _httpClient;
        private string? _pageSize;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiGameService> _logger;
        private IConfiguration _configuration;
        HttpContext _httpContext;

        public ApiGameService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiGameService> logger, IHttpContextAccessor httpContextAccessor, IOptions<UriData> uriDataOptions)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(uriDataOptions.Value.ApiUri);
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<ResponseData<Game>> CreateGameAsync(Game game)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Games");
            var response = await _httpClient.PostAsJsonAsync(uri, game, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Game>>(_serializerOptions);
                return data;
            }
            else
            {
                return new ResponseData<Game>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = $"Объект не добавлен. Error:{response.StatusCode.ToString()}"
                };
            }
        }

        public async Task<ResponseData<bool>> DeleteGameAsync(int id)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Games/{id}")
            };
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return new ResponseData<bool>
                {
                    Success = true,
                    Data = true,
                    ErrorMessage = String.Empty
                };
            }
            else
            {
                return new ResponseData<bool>
                {
                    Success = false,
                    Data = false,
                    ErrorMessage = $"Объект не был удален. Error:{response.StatusCode.ToString()}"
                };
            }
        }
        [AllowAnonymous]
        public async Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Games");
            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                var answer = await response.Content.ReadFromJsonAsync<ResponseData<List<Game>>>();
                return new ResponseData<Game>
                {
                    Success = true,
                    ErrorMessage = null,
                    Data = answer.Data.FirstOrDefault(g => g.Id == id)
                };
            }
            else
            {
                return new ResponseData<Game>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = $"Ошибка: получение игры закончилось с ошибкой. Error:{response.StatusCode.ToString()}"
                };
            }
        }
        [AllowAnonymous]
        public async Task<ResponseData<List<Game>>> GetFullGameListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Games/");
            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                var answer = await response.Content.ReadFromJsonAsync<ResponseData<List<Game>>>();
                return answer;
            }
            return new ResponseData<List<Game>>
            {
                Success = false,
                Data = null,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
            };
        }

        [AllowAnonymous]
        public async Task<ResponseData<ListModel<Game>>> GetGameListAsync(string categoryNormalizedName = "all", int pageNo = 1, int pageSize = 3)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Games/");
            // добавить категорию в маршрут
            urlString.Append($"{categoryNormalizedName}/");
            urlString.Append($"page{pageNo}/");
            // добавить размер страницы в строку запроса
            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", pageSize.ToString()));
            }
            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                var answer = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Game>>>();
                return answer;
            }
            else
            {
                return new ResponseData<ListModel<Game>>
                {
                    Success = false,
                    ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
                };
            }
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile image)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

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
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return new ResponseData<string>()
                {
                    Success = true,
                    Data = null,
                    ErrorMessage = null
                };
            }
            else
            {
                return new ResponseData<string>()
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = $"{image.FileName} не была опубликована"
                };
            }
        }

        public async Task<ResponseData<Game>> UpdateGameAsync(int id, Game game)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var gameJson = JsonConvert.SerializeObject(game); // Сериализуем объект game в JSON

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Games/{id}"),
                Content = new StringContent(gameJson, Encoding.UTF8, "application/json") // Добавляем JSON к телу запроса

            };
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var answer = await response.Content.ReadFromJsonAsync<ResponseData<Game>>();
                return answer;
            }
            else
            {
                return new ResponseData<Game>()
                {
                    Success = false,
                    ErrorMessage = $"Ошибка при обновлении игры с id = {id}",
                    Data = null
                };
            }
        }
    }
}
