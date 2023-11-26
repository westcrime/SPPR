using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;

namespace Web_153502_Tolstoi.API.Services
{
    public class CategoryService : ICategoryService
    {
        AppDbContext _context;
        public CategoryService(AppDbContext context) => _context = context;
        public Task<ResponseData<ListModel<Category>>> GetCategoryListAsync()
        {
            if (_context.Categories == null)
            {
                return Task.FromResult(new ResponseData<ListModel<Category>>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Error! Categories list is null"
                });
            }
            return Task.FromResult(new ResponseData<ListModel<Category>>
            {
               Success = true,
               Data = new ListModel<Category>()
               {
                   Items = _context.Categories.ToList()
               },
               ErrorMessage = string.Empty
            });
        }
    }
}

