using Application.DTOs.Request.Deck;
using Application.DTOs.Response.Deck;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class DeckMapper : Profile
    {
        public DeckMapper()
        {
            CreateMap<FlashcardSideRequest, FlashcardSide>();

            CreateMap<CreateFlashcardRequest, Flashcard>();

            CreateMap<CreateDeckRequest, Deck>();

            CreateMap<Deck, DeckResponse>()
            .ForMember(dest => dest.FlashcardCount, opt => opt.MapFrom(src => src.Flashcards.Count));
        }
    }
}