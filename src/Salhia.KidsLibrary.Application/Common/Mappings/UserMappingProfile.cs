using AutoMapper;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Features.Users.Commands.RegisterUser;
using Salhia.KidsLibrary.Application.Features.Users.Commands.UpdateUser;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // Commands
        CreateMap<RegisterUserCommand, RegisterUserRequest>();

        CreateMap<UpdateUserCommand, UpdateUserRequest>();


        // Queries 
        CreateMap<AppUser, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore())
            .ForMember(dest => dest.UserTypeValue, opt => opt.MapFrom(src => (int)src.UserType))
            .ForMember(dest => dest.UserTypeName, opt => opt.MapFrom(src => src.UserType.ToString()));

    }
}
