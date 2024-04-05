using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSingleton(
            provider => new MapperConfiguration(
                cfg => { cfg.AddProfile(new AutoMapperProfiles(provider.GetService<IConfiguration>())); }
            ).CreateMapper()
        );
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IPhotoService, PhotoService>();

        return services;
    }
}
