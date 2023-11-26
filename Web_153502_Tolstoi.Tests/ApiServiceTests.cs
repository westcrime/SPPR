using Humanizer.Localisation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.API.Services;
using Web_153502_Tolstoi.Domain.Entities;
using Xunit.Abstractions;

namespace Web_153502_Tolstoi.Tests
{
    public class ApiGameServiceTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly ITestOutputHelper output;

        AppDbContext CreateContext() => new AppDbContext(_options);

        public ApiGameServiceTests(ITestOutputHelper output)
        {
            this.output = output;

            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;
            using var context = CreateContext();
            if (context.Database.EnsureCreated())
                context.Database.EnsureCreated();
            List<Category> Categorys = new List<Category>() { new Category() { Id = 1, Name = "FPS", NormalizedName = "FPS" }, new Category() { Id = 2, Name = "Simulator", NormalizedName = "Simulator" }, new Category() { Id = 3, Name = "Strategy", NormalizedName = "Strategy" }, new Category() { Id = 4, Name = "Rogue-Like", NormalizedName = "RogueLike" } };
            context.AddRange(Categorys);
            context.SaveChanges();
            context.AddRange(
                new Game() { Name = "FirstName", CategoryId = Categorys.First(g => g.Name == "FPS").Id, Price = 40.99, Image = $"url/images/FirstName.jpg", Description = "" },
                new Game() { Name = "SecondName", CategoryId = Categorys.First(g => g.Name == "Simulator").Id, Price = 40.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "ThirdName", CategoryId = Categorys.First(g => g.Name == "Strategy").Id, Price = 40.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "FourthName", CategoryId = Categorys.First(g => g.Name == "FPS").Id, Price = 14.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "FifthName", CategoryId = Categorys.First(g => g.Name == "Strategy").Id, Price = 74.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "SixthName", CategoryId = Categorys.First(g => g.Name == "Rogue-Like").Id, Price = 24.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "SeventhName", CategoryId = Categorys.First(g => g.Name == "Rogue-Like").Id, Price = 18.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "FirstName", CategoryId = Categorys.First(g => g.Name == "FPS").Id, Price = 40.99, Image = $"url/images/FirstName.jpg", Description = "" },
                new Game() { Name = "SecondName", CategoryId = Categorys.First(g => g.Name == "Simulator").Id, Price = 40.99, Image = $"url/images/SecondName.jpg" , Description = "" },
                new Game() { Name = "ThirdName", CategoryId = Categorys.First(g => g.Name == "Strategy").Id, Price = 40.99, Image = $"url/images/SecondName.jpg" , Description = "" },
                new Game() { Name = "FourthName", CategoryId = Categorys.First(g => g.Name == "FPS").Id, Price = 14.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "FifthName", CategoryId = Categorys.First(g => g.Name == "Strategy").Id, Price = 74.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "SixthName", CategoryId = Categorys.First(g => g.Name == "Rogue-Like").Id, Price = 24.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "SeventhName", CategoryId = Categorys.First(g => g.Name == "Rogue-Like").Id, Price = 18.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "FirstName", CategoryId = Categorys.First(g => g.Name == "FPS").Id, Price = 40.99, Image = $"url/images/FirstName.jpg", Description = "" },
                new Game() { Name = "SecondName", CategoryId = Categorys.First(g => g.Name == "Simulator").Id, Price = 40.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "ThirdName", CategoryId = Categorys.First(g => g.Name == "Strategy").Id, Price = 40.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "FourthName", CategoryId = Categorys.First(g => g.Name == "FPS").Id, Price = 14.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "FifthName", CategoryId = Categorys.First(g => g.Name == "Strategy").Id, Price = 74.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "SixthName", CategoryId = Categorys.First(g => g.Name == "Rogue-Like").Id, Price = 24.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "SeventhName", CategoryId = Categorys.First(g => g.Name == "Rogue-Like").Id, Price = 18.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "FirstName", CategoryId = Categorys.First(g => g.Name == "FPS").Id, Price = 40.99, Image = $"url/images/FirstName.jpg", Description = "" },
                new Game() { Name = "SecondName", CategoryId = Categorys.First(g => g.Name == "Simulator").Id, Price = 40.99, Image = $"url/images/SecondName.jpg", Description = "" },
                new Game() { Name = "ThirdName", CategoryId = Categorys.First(g => g.Name == "Strategy").Id, Price = 40.99, Image = $"url/images/SecondName.jpg", Description = "" }
                );

            context.SaveChanges();


        }


        public void Dispose()
        {
            _connection.Dispose();
        }


        [Fact]
        public void DefaultCallTest()
        {
            var env = new Mock<IWebHostEnvironment>();
            var conf = new Mock<IConfiguration>();
            var log = new Mock<Microsoft.Extensions.Logging.ILogger<GameService>>();
            var accessor = new Mock<IHttpContextAccessor>();
            var context = CreateContext();

            var service = new GameService(context, env.Object, accessor.Object);

            var data = service.GetGameListAsync("all").Result.Data;

            Assert.Equal(1, data.CurrentPage);
            Assert.Equal(3, data.Items.Count);
            Assert.Equal(8, data.TotalPages);
            Assert.Equal(context.Games.First(), data.Items.First());

        }

        [Fact]
        public void CurrentPageTest()
        {
            var env = new Mock<IWebHostEnvironment>();
            var conf = new Mock<IConfiguration>();
            var log = new Mock<Microsoft.Extensions.Logging.ILogger<GameService>>();
            var accessor = new Mock<IHttpContextAccessor>();
            var context = CreateContext();

            var service = new GameService(context, env.Object, accessor.Object);

            var data = service.GetGameListAsync("all", 2).Result.Data;

            Assert.Equal(2, data.CurrentPage);
            Assert.Equal(context.Games.ToList()[3], data.Items.First());

        }

        [Fact]
        public void FilterTest()
        {
            var env = new Mock<IWebHostEnvironment>();
            var conf = new Mock<IConfiguration>();
            var log = new Mock<Microsoft.Extensions.Logging.ILogger<GameService>>();
            var accessor = new Mock<IHttpContextAccessor>();
            var context = CreateContext();

            var service = new GameService(context, env.Object, accessor.Object);

            var data = service.GetGameListAsync("FPS", 1, 22).Result.Data;

            Assert.All(data.Items, data => Assert.Equal(1, data.CategoryId));

            data = service.GetGameListAsync("all", 1, 22).Result.Data;
            Assert.Equal(2, data.TotalPages);

        }
        [Fact]
        public void FalsePage()
        {
            var env = new Mock<IWebHostEnvironment>();
            var conf = new Mock<IConfiguration>();
            var log = new Mock<Microsoft.Extensions.Logging.ILogger<GameService>>();
            var accessor = new Mock<IHttpContextAccessor>();
            var context = CreateContext();

            var service = new GameService(context, env.Object, accessor.Object);

            var data = service.GetGameListAsync("all", 3, 22).Result;
            Assert.False(data.Success);

        }
    }
}
