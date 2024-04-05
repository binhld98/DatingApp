using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles(IConfiguration configuration)
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(
                dst => dst.PhotoUrl,
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.isMain).Url + "?" + configuration["R_SAS"])
            )
            .ForMember(
                dst => dst.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge())
            );
        CreateMap<Photo, PhotoDto>()
            .ForMember(
                dst => dst.Url,
                opt => opt.MapFrom(src => src.Url + "?" + configuration["R_SAS"])
            );
        CreateMap<MemberUpdateDto, AppUser>();
    }
}
