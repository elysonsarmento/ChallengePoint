using AutoMapper;
using ChallengePoint.Application.Mapper;
using ChallengePoint.Domain.DTO;
using ChallengePoint.Domain.Models;
using Xunit;

namespace ChallengePoint.Tests.Application.Mapper
{
    public class TimekeepingDomainToDTOTests
    {
        private readonly IMapper _mapper;

        public TimekeepingDomainToDTOTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TimekeepingDomainToDTO>();
            });
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void ShouldMapTimekeepingModelToTimekeepingDTO()
        {
            // Arrange
            var timekeepingModel = new TimekeepingModel
            {
                Id = 1,
                CollaboratorId = 2,
                ClockIn = DateTime.Now.AddHours(-8),
                ClockOut = DateTime.Now,
            };

            // Act
            var timekeepingDTO = _mapper.Map<TimekeepingDTO>(timekeepingModel);

            // Assert
            Assert.Equal(timekeepingModel.Id, timekeepingDTO.Id);
            Assert.Equal(timekeepingModel.ClockIn, timekeepingDTO.ClockIn);
            Assert.Equal(timekeepingModel.ClockOut, timekeepingDTO.ClockOut);
        }

        [Fact]
        public void ShouldMapTimekeepingDTOToTimekeepingModel()
        {
            // Arrange
            var timekeepingDTO = new TimekeepingDTO
            {
                Id = 1,
                CollaboratorId = 2,
                ClockIn = DateTime.Now.AddHours(-8),
                ClockOut = DateTime.Now
            };

            // Act
            var timekeepingModel = _mapper.Map<TimekeepingModel>(timekeepingDTO);

            // Assert
            Assert.Equal(timekeepingDTO.Id, timekeepingModel.Id);
            Assert.Equal(timekeepingDTO.ClockIn, timekeepingModel.ClockIn);
            Assert.Equal(timekeepingDTO.ClockOut, timekeepingModel.ClockOut);
        }
    }
}