using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Enitites;
using WebAPI.Controllers;


namespace ActorsTests
{
    public class ActorsAddTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly ActorController _controller;

        public ActorsAddTests()
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
        public async Task AddActor_AddsNewActor()
        {
            // Arrange
            var newActor = new Actor { FirstName = "Anna", LastName = "Nowicka" };

            // Act
            var result = await _controller.AddActor(newActor);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actors = Assert.IsType<List<Actor>>(okResult.Value);
            var expectedCount = await _context.Actors.CountAsync();
            Assert.Equal(3, expectedCount); // Ponieważ dodajemy jednego aktora do istniejących dwóch
            Assert.Contains(actors, a => a.FirstName == "Anna" && a.LastName == "Nowicka");
        }
    }
}
