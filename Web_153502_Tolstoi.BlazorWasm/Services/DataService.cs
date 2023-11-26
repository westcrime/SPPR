using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web_153502_Tolstoi.Domain.Models;
using Web_153502_Tolstoi.Domain.Models;
using Web_153502_Tolstoi.Domain.Entities;

namespace Web_153502_Tolstoi.BlazorWasm.Services
{
    public class DataService : IDataService
    {

        public List<Category> Categorys { get; set; }
        public List<Game> Games { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        private readonly int apiPageSize;
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _tokenProvider;

        public event Action DataLoaded;

        public DataService(IConfiguration config, HttpClient httpClient, IAccessTokenProvider provider)
        {
            apiPageSize = config.GetValue<int>("ApiRequestParams:PageSize");
            _httpClient = httpClient;
            _tokenProvider = provider;
        }



        public async Task GetCategoryListAsync()
        {
            var httpResponse = await _httpClient.GetAsync("Category");
            var response = await httpResponse.Content.ReadFromJsonAsync<ResponseData<ListModel<Category>>>();
            Success = response.Success;
            ErrorMessage = response.ErrorMessage;
            if (Success)
            {
                Categorys = response.Data.Items;
            }
            DataLoaded?.Invoke();
        }

        public async Task<ResponseData<Game>> GetProductByIdAsync(int id)
        {
            var httpResponse = await _httpClient.GetAsync($"Game/id={id}");

            var response = await httpResponse.Content.ReadFromJsonAsync<ResponseData<Game>>();
            Success = response.Success;
            ErrorMessage = response.ErrorMessage;
            DataLoaded?.Invoke();
            return response;
        }

        public async Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);
            }

            var httpResponse = await _httpClient.GetAsync($"Game/Category={categoryNormalizedName}/page={pageNo}/{apiPageSize}");
            var response = await httpResponse.Content.ReadFromJsonAsync<ResponseData<ListModel<Game>>>();
            Success = response.Success;
            ErrorMessage = response.ErrorMessage;
            if (Success)
            {
                Games = response.Data.Items;
                TotalPages = response.Data.TotalPages;
                CurrentPage = response.Data.CurrentPage;
            }
            DataLoaded?.Invoke();
        }
    }
}
