using AutoMapper;
using ChallengePoint.Domain.DTO;
using ChallengePoint.Domain.Models;

namespace ChallengePoint.Application.Mapper
{
    public class CollaboratorDomainToDTO : Profile
    {

        public CollaboratorDomainToDTO()
        {
            CreateMap<CollaboratorModel, CollaboratorDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.Enrollment, opt => opt.MapFrom(src => src.Enrollment));


        }
    }
}
