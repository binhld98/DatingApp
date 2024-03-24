using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(
                dst => dst.PhotoUrl,
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.isMain).Url)
            )
            .ForMember(
                dst => dst.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge())
            );
        CreateMap<Photo, PhotoDto>();
    }
}
