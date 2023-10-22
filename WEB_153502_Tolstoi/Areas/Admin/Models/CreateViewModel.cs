using Microsoft.AspNetCore.Mvc;
using Web_153502_Tolstoi.Domain.Entities;

namespace WEB_153502_Tolstoi.Areas.Admin.Models
{
    public class CreateViewModel
    {
        [BindProperty]
        public Game? Game { get; set; }
        [BindProperty]
        public IFormFile? Image { get; set; }
    }
}
