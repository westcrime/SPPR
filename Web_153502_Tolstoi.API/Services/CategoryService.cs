using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;

namespace Web_153502_Tolstoi.API.Services
{
    public class CategoryService : ICategoryService
    {
        AppDbContext _context;
        public CategoryService(AppDbContext context) => _context = context;
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            return Task.FromResult(new ResponseData<List<Category>>
            {
                Data = _context.Categories.ToList()
            });
        }
    }
}

