using Application.DTOs.Request.Deck;
using Application.DTOs.Response.Deck;
using Application.DTOs.Response.UserProgress;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class DeckMapper : Profile
    {
        public DeckMapper()
        {
            CreateMap<FlashcardSideRequest, FlashcardSide>();

            CreateMap<FlashcardSide, FlashcardSideResponse>();

            CreateMap<CreateFlashcardRequest, Flashcard>();

            CreateMap<Flashcard, FlashcardResponse>();

            CreateMap<CreateDeckRequest, Deck>();

            CreateMap<Deck, DeckResponse>();

            CreateMap<Deck, DeckLearningDetailResponse>();

            CreateMap<Deck, GetListDeckResponse>();

            CreateMap<User, OwnerDeckResponse>();
        }
    }
}