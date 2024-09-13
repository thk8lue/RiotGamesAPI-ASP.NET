using AutoMapper;
using RiotAPI.Models.Dtos;
using RiotAPI.Models.Entities;

namespace RiotAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
