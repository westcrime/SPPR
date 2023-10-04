using Microsoft.EntityFrameworkCore;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;

namespace Web_153502_Tolstoi.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // Выполнение миграций
            await context.Database.MigrateAsync();

            await context.Categories.AddAsync(new Category
            {
                Name = "Стратегии",
                NormalizedName = "strategy"
            });
            await context.Categories.AddAsync(new Category
            {
                Name = "Симуляторы",
                NormalizedName = "simulator"
            });

            await context.SaveChangesAsync();
            Console.WriteLine(context.Categories.Count().ToString());
            Category strategyCategory = context.Categories.First(c => c.NormalizedName.Equals("strategy"));
            Category simulatorCategory = context.Categories.First(c => c.NormalizedName.Equals("simulator"));

            await context.Games.AddAsync(new Game
            {
                Name = "Dota 2",
                Description = "Соревновательная MOBA-стратегия",
                Price = 0,
                Image = app.Configuration["Url"] + "/Images/Dota.jpg",
                CategoryId = (int)(strategyCategory.Id)
            });

            await context.Games.AddAsync(new Game
            {
                Name = "Factorio",
                Description = "Симулятор фабрики",
                Price = 14.99,
                Image = app.Configuration["Url"] + "/Images/Factorio.jpg",
                CategoryId = (int)(simulatorCategory.Id)
            });
            await context.Games.AddAsync(new Game
            {
                Name = "Farming simulator 23",
                Description = "Симулятор сельского хозяйства",
                Price = 34.99,
                Image = app.Configuration["Url"] + "/Images/Fs23.jpg",
                CategoryId = (int)(simulatorCategory.Id)
            });
            await context.Games.AddAsync(new Game
            {
                Name = "Fifa 23",
                Description = "Симулятор футбола",
                Price = 44.99,
                Image = app.Configuration["Url"] + "/Images/fifa23.jpg",
                CategoryId = (int)(simulatorCategory.Id)
            });
            await context.Games.AddAsync(new Game
            {
                Name = "StarCraft 2",
                Description = "Стратегия в космическом будущем",
                Price = 0,
                Image = app.Configuration["Url"] + "/Images/starcraft2.jpg",
                CategoryId = (int)(strategyCategory.Id)
            });

            await context.SaveChangesAsync();
        }
    }
}
