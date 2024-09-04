using AutoMapper;
using ChallengePoint.Application.ViewModel;
using ChallengePoint.Controllers;
using ChallengePoint.Domain.DTO;
using ChallengePoint.Domain.Interface;
using ChallengePoint.Domain.Models;
using ChallengePoint.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ChallengePoint.Tests.Controllers
{
    public class CollaboratorControllerTests
    {
        private readonly Mock<ICollaboratorRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CollaboratorController _controller;

        public CollaboratorControllerTests()
        {
            _mockRepo = new Mock<ICollaboratorRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CollaboratorController(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsPaginatedList_WhenCollaboratorsExist()
        {
            // Arrange
            var collaborators = new List<CollaboratorModel> { new CollaboratorModel { Id = 1, Name = "Fulano", Position = "Developer", Enrollment = "123" } };
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(collaborators);
            _mockMapper.Setup(m => m.Map<IEnumerable<CollaboratorDto>>(It.IsAny<IEnumerable<CollaboratorModel>>())).Returns(new List<CollaboratorDto> { new CollaboratorDto { Id = 1, Name = "Fulano" } });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var paginatedResult = Assert.IsType<SimplePagination<CollaboratorDto>>(okResult.Value);
            Assert.Single(paginatedResult.Items);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoCollaboratorsExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((IEnumerable<CollaboratorModel>)null);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_ReturnsCollaborator_WhenCollaboratorExists()
        {
            // Arrange
            var collaborator = new CollaboratorModel { Id = 1, Name = "Fulano", Position = "Developer", Enrollment = "123" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(collaborator);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCollaborator = Assert.IsType<CollaboratorModel>(okResult.Value);
            Assert.Equal(1, returnedCollaborator.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenCollaboratorDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(default(CollaboratorModel));

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task Add_ReturnsCreatedAtAction_WhenCollaboratorIsAdded()
        {
            // Arrange
            var collaboratorViewModel = new CollaboratorViewModel { Name = "Fulano", Position = "Developer", Salary = 50000, Enrollment = "123" };
            var collaboratorModel = new CollaboratorModel { Id = 1, Name = "Fulano", Position = "Developer", Salary = 50000, Enrollment = "123" };
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<CollaboratorModel>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Add(collaboratorViewModel);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_controller.GetById), createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenCollaboratorIsUpdated()
        {
            // Arrange
            var collaboratorViewModel = new CollaboratorViewModel { Name = "Fulano", Position = "Developer", Salary = 50000, Enrollment = "123" };
            var collaboratorModel = new CollaboratorModel { Id = 1, Name = "Fulano", Position = "Developer", Salary = 50000, Enrollment = "123" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(collaboratorModel);
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<CollaboratorModel>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, collaboratorViewModel);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenCollaboratorDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((CollaboratorModel)null);

            // Act
            var result = await _controller.Update(1, new CollaboratorViewModel { Name = "Fulano", Position = "Developer", Enrollment = "123" });

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenCollaboratorIsDeleted()
        {
            // Arrange
            var collaboratorModel = new CollaboratorModel { Id = 1, Name = "Fulano", Position = "Developer", Enrollment = "123" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(collaboratorModel);
            _mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenCollaboratorDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((CollaboratorModel)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}