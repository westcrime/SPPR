using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.API.Services;
using Web_153502_Tolstoi.Domain.Entities;

namespace WEB_153502_Tolstoi.Areas.Admin.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IGameService _gameService;

        public IndexModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public IList<Game> Game { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Game = (await _gameService.GetFullGameListAsync()).Data;
        }
    }
}
