using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;

namespace Web_153502_Tolstoi.API.Services
{
    public class GameService : IGameService
    {
        private readonly int _maxPageSize = 20;
        AppDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        private IHttpContextAccessor _httpContextAccessor;

        public GameService(AppDbContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseData<Game>> CreateGameAsync(Game game)
        {
            if (game != null)
            {
                await _context.Database.MigrateAsync();
                await _context.SaveChangesAsync();
                await _context.Games.AddAsync(game);
                await _context.SaveChangesAsync();
                return new ResponseData<Game>()
                {
                    Success = true,
                    Data = game,
                    ErrorMessage = null
                };
            }
            else
            {
                return new ResponseData<Game>()
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "game is null pointer"
                };
            }
        }

        public async Task<ResponseData<bool>> DeleteGameAsync(int id)
        {
            if (id != 0)
            {
                var game = await _context.Games.FindAsync(id);
                if (game != null)
                {
                    await _context.Database.MigrateAsync();
                    await _context.SaveChangesAsync();
                    _context.Games.Remove(game);
                    await _context.SaveChangesAsync();
                    return new ResponseData<bool>()
                    {
                        Success = true,
                        Data = true,
                        ErrorMessage = String.Empty
                    };
                }
                else
                {
                    return new ResponseData<bool>()
                    {
                        Success = false,
                        Data = false,
                        ErrorMessage = $"game with id {id} is not found"
                    };
                }
            }
            else
            {
                return new ResponseData<bool>()
                {
                    Success = false,
                    Data = false,
                    ErrorMessage = "id equals to 0"
                };
            }
        }

        public async Task<ResponseData<List<Game>>> GetFullGameListAsync()
        {
            var games = await _context.Games.ToListAsync();
            return new ResponseData<List<Game>>
            {
                Data = games,
                Success = true,
                ErrorMessage = null
            };
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

        public async Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            if (id == 0)
            {
                return new ResponseData<Game>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = $"id cant be null (id = {id}"
                };
            }
            await _context.Database.MigrateAsync();
            await _context.SaveChangesAsync();
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return new ResponseData<Game>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = $"game with {id} is not found"
                };
            }
            return new ResponseData<Game>()
            {
                Data = game,
                Success = true,
                ErrorMessage = null
            };
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var responseData = new ResponseData<string>();
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                responseData.Success = false;
                responseData.ErrorMessage = "No item found";
                return responseData;
            }
            var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;
            var imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            if (formFile != null)
                // Удалить предыдущее изображение
                if (!String.IsNullOrEmpty(game.Image))
                {
                    var prevImage = Path.Combine(imageFolder, Path.GetFileName(game.Image));
                    if (File.Exists(prevImage))
                    {
                        File.Delete(prevImage); // Удаляем файл, если он существует
                    }
                }
            // Создать имя файла
            var ext = Path.GetExtension(formFile.FileName);
            var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
            // Сохранить файл
            using (var stream = new FileStream(Path.Combine(imageFolder, fName), FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            // Указать имя файла в объекте
            game.Image = $"{host}/Images/{fName}";
            await _context.SaveChangesAsync();
            responseData.Data = game.Image;
            return responseData;
        }

        public async Task<ResponseData<Game>> UpdateGameAsync(int id, Game game)
        {
            await _context.Database.MigrateAsync();
            await _context.SaveChangesAsync();
            var gameToUpdate = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (gameToUpdate != null)
            {
                gameToUpdate.Name = game.Name;
                gameToUpdate.Description = game.Description;
                gameToUpdate.Price = game.Price;
                gameToUpdate.CategoryId = game.CategoryId;
                _context.Games.Update(gameToUpdate);
                await _context.SaveChangesAsync();
                return new ResponseData<Game>()
                {
                    Data = game,
                    Success = true,
                    ErrorMessage = String.Empty
                };
            }
            else
            {
                return new ResponseData<Game>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = $"cant find a game to update with id = {id}"
                };
            }
        }
    }
}
