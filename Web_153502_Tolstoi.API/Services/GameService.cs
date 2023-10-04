using Microsoft.EntityFrameworkCore;
using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;

namespace Web_153502_Tolstoi.API.Services
{
    public class GameService : IGameService
    {
        private readonly int _maxPageSize = 20;
        AppDbContext _context;

        public GameService(AppDbContext context)
        {
            _context = context;
        }

        public Task<ResponseData<Game>> CreateProductAsync(Game product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;
            var _games = await _context.Games.ToListAsync();
            var _categories = await _context.Categories.ToListAsync();
            if (categoryNormalizedName != null)
            {
                if (_categories.FindAll(c => c.NormalizedName == categoryNormalizedName).Count == 0)
                    return new ResponseData<ListModel<Game>>
                    {
                        Data = null,
                        Success = false,
                        ErrorMessage = "No such category"
                    };
                var totalPages = (int)Math.Ceiling((double)_games.Where(game => game.CategoryId == _categories.Find(c => c.NormalizedName.Equals(categoryNormalizedName)).Id).ToList().Count / pageSize);
                if (pageNo > totalPages)
                    return new ResponseData<ListModel<Game>>
                    {
                        Data = null,
                        Success = false,
                        ErrorMessage = "No such page"
                    };
                Console.WriteLine(new ResponseData<ListModel<Game>>
                {
                    Data = new ListModel<Game>
                    {
                        Items = _games.Where(game => game.CategoryId == _categories.Find(c => c.NormalizedName.Equals(categoryNormalizedName)).Id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(),
                        CurrentPage = pageNo,
                        TotalPages = totalPages
                    },
                }.Data.Items.ToString());
                return new ResponseData<ListModel<Game>>
                {
                    Data = new ListModel<Game>
                    {
                        Items = _games.Where(game => game.CategoryId == _categories.Find(c => c.NormalizedName.Equals(categoryNormalizedName)).Id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(),
                        CurrentPage = pageNo,
                        TotalPages = totalPages
                    },
                };
            }
            else
            {
                var totalPages = (int)Math.Ceiling((double)_games.Count / pageSize);
                if (pageNo > totalPages)
                    return new ResponseData<ListModel<Game>>
                    {
                        Data = null,
                        Success = false,
                        ErrorMessage = "No such page"
                    };
                return new ResponseData<ListModel<Game>>
                {
                    Data = new ListModel<Game>
                    {
                        Items = _games.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(),
                        CurrentPage = pageNo,
                        TotalPages = totalPages
                    }
                };
            }
        }

        public Task<ResponseData<Game>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int id, Game product)
        {
            throw new NotImplementedException();
        }
    }
}
