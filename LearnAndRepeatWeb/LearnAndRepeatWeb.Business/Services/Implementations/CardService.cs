using AutoMapper;
using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Contracts.Events.Card;
using LearnAndRepeatWeb.Contracts.Requests.Card;
using LearnAndRepeatWeb.Contracts.Responses.Card;
using LearnAndRepeatWeb.Infrastructure.Entities.Card;
using LearnAndRepeatWeb.Infrastructure.Repositories.Card;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Business.Services.Implementations
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;
        private readonly IBusControl _busControl;
        private readonly IUserAuthorizationService _userAuthorizationService;

        public CardService(ICardRepository cardRepository, IMapper mapper, IBusControl busControl, IUserAuthorizationService userAuthorizationService)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
            _busControl = busControl;
            _userAuthorizationService = userAuthorizationService;
        }

        public async Task<CardResponse> PostCard(string userKey, PostCardRequest postCardRequest)
        {
            _userAuthorizationService.CheckUserPermission(userKey);
            long userId = _userAuthorizationService.ConvertUserKeyToUserResponse(userKey).Id;

            CardModel cardModel = new CardModel
            {
                UserId = userId,
                Content = postCardRequest.Content,
                Header = postCardRequest.Header,
                Tag = postCardRequest.Tag,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _cardRepository.Add(cardModel);

            CardResponse cardResponse = _mapper.Map<CardResponse>(cardModel);

            await _busControl.Publish(new CardCreatedEvent
            {
                CardResponse = cardResponse
            });

            return cardResponse;
        }
    }
}
