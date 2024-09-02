using AutoMapper;
using ChallengePoint.Application.Mapper;
using ChallengePoint.Domain.DTO;
using ChallengePoint.Domain.Models;
using Xunit;

namespace ChallengePoint.Application.test.Mappings
{
    public class CollaboratorDomainToDTOTests
    {
        private readonly IMapper _mapper;

        public CollaboratorDomainToDTOTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CollaboratorDomainToDTO>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void CollaboratorDomainToDTO_ConfigurationIsValid()
        {
            // Act & Assert
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void CollaboratorDomainToDTO_MapsCorrectly()
        {
            // Arrange
            var collaboratorModel = new CollaboratorModel
            {
                Id = 1,
                Name = "Fulano",
                Position = "Developer",
                Salary = 50000,
                Enrollment = "A12345"
            };

            // Act
            var collaboratorDto = _mapper.Map<CollaboratorDto>(collaboratorModel);

            // Assert
            Assert.Equal(collaboratorModel.Id, collaboratorDto.Id);
            Assert.Equal(collaboratorModel.Name, collaboratorDto.Name);
            Assert.Equal(collaboratorModel.Position, collaboratorDto.Position);
            Assert.Equal(collaboratorModel.Salary, collaboratorDto.Salary);
            Assert.Equal(collaboratorModel.Enrollment, collaboratorDto.Enrollment);
        }
    }
}
