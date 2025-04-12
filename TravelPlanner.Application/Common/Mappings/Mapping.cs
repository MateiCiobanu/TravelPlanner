using AutoMapper;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Application.Common.Mapping
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserCreateDTO, User>();
        }
    }
}
