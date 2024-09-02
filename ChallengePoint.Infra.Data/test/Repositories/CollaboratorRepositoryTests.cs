using ChallengePoint.Domain.Models;
using ChallengePoint.Infra.Data.Context;
using ChallengePoint.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ChallengePoint.Tests.Repositories
{
    public class CollaboratorRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public CollaboratorRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Collaborator()
        {
            // Arrange
            var collaborator = new CollaboratorModel
            {
                Id = 1,
                Name = "Fulano",
                Position = "Developer",
                Salary = 60000,
                Enrollment = "12345"
            };

            await using var context = new AppDbContext(_dbContextOptions);
            var repository = new CollaboratorRepository(context);

            // Act
            await repository.AddAsync(collaborator);
            var result = await context.Collaborators.FindAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(collaborator.Name, result.Name);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Collaborator()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_DeleteAsync")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var repository = new CollaboratorRepository(context);

                var collaborator = new CollaboratorModel
                {
                    Name = "Fulano",
                    Position = "Developer",
                    Salary = 50000,
                    Enrollment = "12345"
                };

                await repository.AddAsync(collaborator);
            }

            // Act & Assert
            using (var context = new AppDbContext(options))
            {
                var repository = new CollaboratorRepository(context);
                var collaboratorToRemove = await repository.GetByEnrollmentAsync("12345");
                await repository.DeleteAsync(collaboratorToRemove.Id);

                var deletedCollaborator = await repository.GetByIdAsync(collaboratorToRemove.Id);
                Assert.Null(deletedCollaborator);
            }
        }


        [Fact]
        public async Task GetAllAsync_Should_Return_Paginated_Results()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDbContext(options))
            {
                var repository = new CollaboratorRepository(context);

                var collaborator1 = new CollaboratorModel
                {
                    Id = 1,
                    Name = "Fulano",
                    Position = "Developer",
                    Salary = 50000,
                    Enrollment = "12345"
                };

                var collaborator2 = new CollaboratorModel
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Position = "Designer",
                    Salary = 55000,
                    Enrollment = "67890"
                };

                await repository.AddAsync(collaborator1);
                await repository.AddAsync(collaborator2);
            }

            // Act & Assert
            using (var context = new AppDbContext(options))
            {
                var repository = new CollaboratorRepository(context);
                var result = await repository.GetAllAsync(1, 1);

                Assert.Single(result);
                Assert.Equal("Fulano", result.First().Name);
            }
        }


        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_Collaborator()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDbContext(options))
            {
                var repository = new CollaboratorRepository(context);

                var collaborator = new CollaboratorModel
                {
                    Id = 1,
                    Name = "Fulano",
                    Position = "Developer",
                    Salary = 50000,
                    Enrollment = "12345"
                };

                await repository.AddAsync(collaborator);
            }

            // Act & Assert
            using (var context = new AppDbContext(options))
            {
                var repository = new CollaboratorRepository(context);
                var result = await repository.GetByIdAsync(1);

                Assert.NotNull(result);
                Assert.Equal("Fulano", result.Name);
            }
        }


        [Fact]
        public async Task GetByEnrollmentAsync_Should_Return_Correct_Collaborator()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Collaborators.RemoveRange(context.Collaborators);
                await context.SaveChangesAsync();

                var repository = new CollaboratorRepository(context);

                var collaborator = new CollaboratorModel
                {
                    Id = 1,
                    Name = "John Doe",
                    Position = "Developer",
                    Salary = 50000,
                    Enrollment = "12345"
                };

                await repository.AddAsync(collaborator);
            }

            // Act & Assert
            using (var context = new AppDbContext(options))
            {
                var repository = new CollaboratorRepository(context);
                var result = await repository.GetByEnrollmentAsync("12345");

                Assert.NotNull(result);
                Assert.Equal("John Doe", result.Name);
            }
        }


        [Fact]
        public async Task UpdateAsync_Should_Update_Collaborator_Details()
        {
            // Arrange
            var collaborator = new CollaboratorModel
            {
                Id = 1,
                Name = "Fulano",
                Position = "Developer",
                Salary = 60000,
                Enrollment = "12345"
            };

            await using var context = new AppDbContext(_dbContextOptions);
            await context.Collaborators.AddAsync(collaborator);
            await context.SaveChangesAsync();

            context.Entry(collaborator).State = EntityState.Detached;

            var repository = new CollaboratorRepository(context);

            // Act
            collaborator.Name = "Fulano 2";
            await repository.UpdateAsync(collaborator);
            var result = await context.Collaborators.FindAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Fulano 2", result.Name);
        }

    }
}
