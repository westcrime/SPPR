using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;

namespace WEB_153502_Tolstoi.Services.CategoryServices
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
