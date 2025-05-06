using AutoMapper;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Application.Common.Mapping
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserCreateDTO, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.TravelerTypes, opt => opt.Ignore()); ;
        }
    }
}
