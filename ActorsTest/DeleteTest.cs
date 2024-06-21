using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Enitites;
using WebAPI.Controllers;
using Xunit;

namespace ActorsTests
{
    public class ActorsDeleteTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly ActorController _controller;

        public ActorsDeleteTests()
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
        public async Task DeleteActor_DeletesExistingActor()
        {
            // Arrange
            var existingActor = await _context.Actors.FirstAsync();

            // Act
            var result = await _controller.DeleteActor(existingActor.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actors = Assert.IsType<List<Actor>>(okResult.Value);
            var dbActor = await _context.Actors.FindAsync(existingActor.Id);

            Assert.Null(dbActor); // Sprawdzamy, czy aktor został usunięty z bazy danych
            var expectedCount = await _context.Actors.CountAsync();
            Assert.Equal(1, expectedCount); // Ponieważ usuwamy jednego aktora z dwóch
        }
    }
}
