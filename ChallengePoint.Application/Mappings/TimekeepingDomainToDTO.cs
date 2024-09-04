using AutoMapper;
using ChallengePoint.Domain.DTO;
using ChallengePoint.Domain.Models;

namespace ChallengePoint.Application.Mapper
{
    public class TimekeepingDomainToDTO : Profile
    {
        public TimekeepingDomainToDTO()
        {
            CreateMap<TimekeepingModel, TimekeepingDTO>();
            CreateMap<TimekeepingDTO, TimekeepingModel>();
        }
    }
}
