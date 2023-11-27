using Microsoft.AspNetCore.Mvc;
using Moq;
using WEB_153502_Tolstoi.Services.Api.Services;
using WEB_153502_Tolstoi.Controllers;
using Web_153502_Tolstoi.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Web_153502_Tolstoi.Domain.Models;
using Web_153502_Tolstoi.Domain.Entities;

namespace Web_153502_Tolstoi.Tests
{
    public class GameControllerTests
    {
        private readonly ITestOutputHelper output;
        public GameControllerTests(ITestOutputHelper output) { this.output = output; }


        [Fact]
        public void ErrorCategory()
        {
            var gameService = new Mock<IGameService>();
            var categoryService = new Mock<ICategoryService>();

            var data = Task.FromResult(new ResponseData<ListModel<Category>>() { Data = new(), Success = false, ErrorMessage = "Category error message" });
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(() => data);

            GameController controller = new GameController(gameService.Object, categoryService.Object);

            // Act
            var result = controller.Index("all", 1).Result;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category error message", (result as NotFoundObjectResult).Value);

        }

        [Fact]
        public void ErrorGame()
        {
            var gameService = new Mock<IGameService>();
            var categoryService = new Mock<ICategoryService>();


            var categoryData = Task.FromResult(new ResponseData<ListModel<Category>>() { Data = new(), Success = true, ErrorMessage = "" });
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(() => categoryData);

            var gameData = Task.FromResult(new ResponseData<ListModel<Game>>() { Data = new(), Success = false, ErrorMessage = "Game error message" });
            gameService.Setup(m => m.GetGameListAsync(It.IsAny<string>(), It.IsAny<int>(), 3)).Returns(() => gameData);


            GameController controller = new GameController(gameService.Object, categoryService.Object);

            // Act
            var result = controller.Index("all", 1).Result;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Game error message", (result as NotFoundObjectResult).Value);

        }

        [Fact]
        public void ViewCategorys()
        {
            var gameService = new Mock<IGameService>();
            var categoryService = new Mock<ICategoryService>();


            var categoryData = Task.FromResult(new ResponseData<ListModel<Category>>() { Data = new ListModel<Category>() { Items = new List<Category>() { new Category() { Name = "CategoryName" } } }, Success = true, ErrorMessage = "Category error message" });
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(() => categoryData);

            var gameData = Task.FromResult(new ResponseData<ListModel<Game>>() { Data = new ListModel<Game>() { Items = new List<Game>() { new Game() { Name = "GameName" } } }, Success = true, ErrorMessage = "Game error message" });
            gameService.Setup(m => m.GetGameListAsync(It.IsAny<string>(), It.IsAny<int>(), 3)).Returns(() => gameData);


            GameController controller = new GameController(gameService.Object, categoryService.Object);

            // Act
            var result = controller.Index("all", 1).Result;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(categoryData.Result.Data.Items, (result as ViewResult).ViewData["Categories"]);
            Assert.Equal("all", (result as ViewResult).ViewData["currentCategory"]);
            Assert.Equal(gameData.Result.Data, (result as ViewResult).Model);
        }

    }
}
