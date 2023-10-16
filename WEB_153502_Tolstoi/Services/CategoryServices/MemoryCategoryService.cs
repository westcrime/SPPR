using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;

namespace WEB_153502_Tolstoi.Services.CategoryServices
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category 
                {
                    Id = 1,
                    Name = "Стратегии", 
                    NormalizedName = "strategy"
                },
                new Category 
                {
                    Id = 2,
                    Name = "Симуляторы",
                    NormalizedName = "simulator"
                }
            };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
    }
}
