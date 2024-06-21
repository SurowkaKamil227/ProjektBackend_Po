using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Enitites;
using WebAPI.Controllers;
using Xunit;

namespace ActorsTests
{
    public class ActorsGetTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly ActorController _controller;

        public ActorsGetTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new DataContext(options);
            _context.Database.EnsureCreated();
            _controller = new ActorController(_context);

            _context.Actors.AddRange(
                new Actor { FirstName = "Jan", LastName = "Kowalski" },
                new Actor { FirstName = "Paweł", LastName = "Nowak" }
            );
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetActor_ReturnsExistingActor()
        {
            // Arrange
            var existingActor = await _context.Actors.FirstAsync();

            // Act
            var result = await _controller.GetActor(existingActor.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actor = Assert.IsType<Actor>(okResult.Value);

            Assert.NotNull(actor);
            Assert.Equal(existingActor.Id, actor.Id);
            Assert.Equal(existingActor.FirstName, actor.FirstName);
            Assert.Equal(existingActor.LastName, actor.LastName);
        }

        [Fact]
        public async Task GetActor_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetActor(999); // Nieistniejący Id

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Actor not found", notFoundResult.Value);
        }
    }
}
