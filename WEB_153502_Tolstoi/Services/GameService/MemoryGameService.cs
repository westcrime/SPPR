using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Linq;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;
using WEB_153502_Tolstoi.Services.CategoryServices;

namespace WEB_153502_Tolstoi.Services.GameService
{
    public class MemoryGameService : IGameService
    {
        List<Game> _games;
        List<Category> _categories;
        IConfiguration _configuration;

        public MemoryGameService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data;
            _configuration = config;
            SetupData();
        }

        public Task<ResponseData<Game>> CreateProductAsync(Game product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? categoryNormalizedName = null, int pageNo = 1)
        {
            if (categoryNormalizedName != null)
            {
                var numberOfPages = (int)Math.Ceiling((double)_games.Where(game => game.CategoryId == _categories.Find(c => c.NormalizedName.Equals(categoryNormalizedName)).Id).ToList().Count / _configuration.GetValue<int>("ItemsPerPage"));

                return Task.FromResult(new ResponseData<ListModel<Game>>
                {
                    Data = new ListModel<Game>
                    {
                        Items = _games.Where(game => game.CategoryId == _categories.Find(c => c.NormalizedName.Equals(categoryNormalizedName)).Id).Skip((pageNo - 1) * _configuration.GetValue<int>("ItemsPerPage")).Take(_configuration.GetValue<int>("ItemsPerPage")).ToList(),
                        CurrentPage = pageNo,
                        TotalPages = numberOfPages
                    },
                }) ;
            }
            else
            {
                var numberOfPages = (int)Math.Ceiling((double)_games.Count / _configuration.GetValue<int>("ItemsPerPage"));

                return Task.FromResult(new ResponseData<ListModel<Game>> 
                { 
                    Data = new ListModel<Game> 
                    {
                        Items = _games.Skip((pageNo - 1) * _configuration.GetValue<int>("ItemsPerPage")).Take(_configuration.GetValue<int>("ItemsPerPage")).ToList(),
                        CurrentPage = pageNo,
                        TotalPages = numberOfPages
                    } 
                });
            }
        }

        public Task UpdateGameAsync(int id, Game game, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Инициализация списков
        /// </summary>
        private void SetupData()
        {
            _games = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Name = "Dota 2",
                    Description = "Соревновательная MOBA-стратегия",
                    Price = 0,
                    Image = "Images/Dota.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("strategy", StringComparison.Ordinal)).Id
                },
                new Game
                {
                    Id = 2,
                    Name = "Factorio",
                    Description = "Симулятор фабрики",
                    Price = 14.99,
                    Image = "Images/Factorio.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("simulator", StringComparison.Ordinal)).Id
                },
                new Game
                {
                    Id = 3,
                    Name = "Farming simulator 23",
                    Description = "Симулятор сельского хозяйства",
                    Price = 34.99,
                    Image = "Images/Fs23.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("simulator", StringComparison.Ordinal)).Id
                },
                new Game
                {
                    Id = 4,
                    Name = "Fifa 23",
                    Description = "Симулятор футбола",
                    Price = 44.99,
                    Image = "Images/fifa23.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("simulator", StringComparison.Ordinal)).Id
                },
                new Game
                {
                    Id = 5,
                    Name = "StarCraft 2",
                    Description = "Стратегия в космическом будущем",
                    Price = 0,
                    Image = "Images/starcraft2.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("strategy", StringComparison.Ordinal)).Id
                },
            };
        }
    }
}
