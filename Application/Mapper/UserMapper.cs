using Application.DTOs.Request.Auth;
using Application.DTOs.Response.Auth;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<SignUpUserRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<User, UserResponse>();
        }
    }
}