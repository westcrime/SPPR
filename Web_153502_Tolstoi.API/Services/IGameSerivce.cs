using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;

namespace Web_153502_Tolstoi.API.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <param name="pageSize">количество объектов на странице</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns></returns>
        public Task<ResponseData<Game>> GetGameByIdAsync(int id);
        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id">Id изменяемомго объекта</param>
        /// <param name="game">объект с новыми параметрами</param>
        /// <returns></returns>
        public Task UpdateGameAsync(int id, Game game);
        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="id">Id удаляемомго объекта</param>
        /// <returns></returns>
        public Task DeleteGameAsync(int id);
        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="game">Новый объект</param>
        /// <returns>Созданный объект</returns>
        public Task<ResponseData<Game>> CreateGameAsync(Game game, IFormFile formFile);
        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="formFile">файл изображения</param>
        /// <returns>Url к файлу изображения</returns
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);

    }
}
