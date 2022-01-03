using AutoMapper;
using LearnAndRepeatWeb.Contracts.Responses.Card;
using LearnAndRepeatWeb.Infrastructure.Entities.Card;

namespace LearnAndRepeatWeb.Business.Mappers.Card
{
    public class CardMappingProfile : Profile
    {
        public CardMappingProfile()
        {
            CreateMap<CardModel, CardResponse>();
        }
    }
}
