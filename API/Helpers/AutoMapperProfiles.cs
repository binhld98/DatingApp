using API.Configs;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles(IConfiguration configuration)
    {
        var blobStorageConfigs = configuration.GetSection("BlobStorage").Get<BlobStorageConfigs>();

        CreateMap<AppUser, MemberDto>()
            .ForMember(
                dst => dst.PhotoUrl,
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.isMain).Url + "?" + blobStorageConfigs.R_SAS)
            )
            .ForMember(
                dst => dst.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge())
            );
        CreateMap<Photo, PhotoDto>()
            .ForMember(
                dst => dst.Url,
                opt => opt.MapFrom(src => src.Url + "?" + blobStorageConfigs.R_SAS)
            );
        CreateMap<MemberUpdateDto, AppUser>();
        CreateMap<RegisterDto, AppUser>();
    }
}
