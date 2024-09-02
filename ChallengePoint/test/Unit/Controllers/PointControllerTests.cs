using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

namespace ChallengePoint.Tests
{
    public class PointControllerTests
    {
        private readonly Mock<IPointRepository> _mockPointRepository;
        private readonly Mock<ICollaboratorRepository> _mockCollaboratorRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PointController _controller;

        public PointControllerTests()
        {
            _mockPointRepository = new Mock<IPointRepository>();
            _mockCollaboratorRepository = new Mock<ICollaboratorRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new PointController(_mockPointRepository.Object, _mockCollaboratorRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task ClockOut_clockOutIsNull_ReturnsBadRequest()
        {
            var result = await _controller.ClockOut(new ClockOutViewModel { clockOut = null, Enrollment = "123" });
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ClockOut_EnrollmentIsNull_ReturnsBadRequest()
        {
            var result = await _controller.ClockOut(new ClockOutViewModel { clockOut = "2023-10-10T18:00:00Z", Enrollment = null });
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ClockOut_CollaboratorNotFound_ReturnsNotFound()
        {
            _mockCollaboratorRepository.Setup(repo => repo.GetByEnrollmentAsync(It.IsAny<string>())).ReturnsAsync(null as CollaboratorModel);
            var result = await _controller.ClockOut(new ClockOutViewModel { clockOut = "2023-10-10T18:00:00Z", Enrollment = "123" });
            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public async Task ClockOut_NotBusinessDay_ReturnsBadRequest()
        {
            var result = await _controller.ClockOut(new ClockOutViewModel { clockOut = "2023-10-08T18:00:00Z", Enrollment = "123" }); // Domingo
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ClockOut_ValidRequest_ReturnsOk()
        {
            var collaborator = new CollaboratorModel { Id = 1, Enrollment = "123", Name = "Fulano", Position = "CEO" };
            _mockCollaboratorRepository.Setup(repo => repo.GetByEnrollmentAsync(It.IsAny<string>())).ReturnsAsync(collaborator);
            _mockPointRepository.Setup(repo => repo.AddPointAsync(It.IsAny<TimekeepingModel>())).Returns(Task.CompletedTask);

            var result = await _controller.ClockOut(new ClockOutViewModel { clockOut = "2023-10-10T18:00:00Z", Enrollment = "123" });
            Assert.IsType<OkResult>(result);
        }
    }
}