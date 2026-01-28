using Application.DTOs.Request.Folder;
using Application.DTOs.Response.Folder;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class FolderMapper : Profile
    {
        public FolderMapper()
        {
            CreateMap<CreateFolderRequest, Folder>();
            
            CreateMap<Folder, FolderResponse>()
            .ForMember(dest => dest.DeckCount, opt => opt.MapFrom(src => src.Decks.Count));
        }
    }
}