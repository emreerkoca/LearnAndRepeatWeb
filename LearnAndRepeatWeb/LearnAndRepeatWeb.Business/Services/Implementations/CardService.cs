using AutoMapper;
using LearnAndRepeatWeb.Business.CustomExceptions;
using LearnAndRepeatWeb.Business.Resources;
using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Contracts.Events.Card;
using LearnAndRepeatWeb.Contracts.Requests.Card;
using LearnAndRepeatWeb.Contracts.Responses.Card;
using LearnAndRepeatWeb.Infrastructure.Entities.Card;
using LearnAndRepeatWeb.Infrastructure.Repositories.Card;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task PatchCard(string userKey, PatchCardRequest patchCardRequest)
        {
            _userAuthorizationService.CheckUserPermission(userKey);
            long userId = _userAuthorizationService.ConvertUserKeyToUserResponse(userKey).Id;

            var cardModel = await _cardRepository.GetItem(m => m.UserId == userId && m.Id == patchCardRequest.Id);

            if (cardModel == null)
            {
                throw new NotFoundException(Resource.CardCouldNotFound);
            }

            if (!string.IsNullOrEmpty(patchCardRequest.Header))
            {
                cardModel.Header = patchCardRequest.Header;
                cardModel.UpdateDate = DateTime.UtcNow;
            }

            if (!string.IsNullOrEmpty(patchCardRequest.Content))
            {
                cardModel.Content = patchCardRequest.Content;
                cardModel.UpdateDate = DateTime.UtcNow;
            }

            await _cardRepository.Update(cardModel);
        }

        public async Task<CardListResponse> GetCard(string userKey, GetCardRequest getCardRequest)
        {
            _userAuthorizationService.CheckUserPermission(userKey);
            long userId = _userAuthorizationService.ConvertUserKeyToUserResponse(userKey).Id;

            IQueryable<CardModel> cardsIQueryable = _cardRepository.GetItemsIQueryable();

            cardsIQueryable = cardsIQueryable.Where(m => m.UserId == userId);

            if (getCardRequest.Id.HasValue)
            {
                cardsIQueryable = cardsIQueryable.Where(m => m.Id == getCardRequest.Id);
            }

            if (!string.IsNullOrEmpty(getCardRequest.Header))
            {
                cardsIQueryable = cardsIQueryable.Where(m => m.Header.Equals(getCardRequest.Header));
            }

            if (!string.IsNullOrEmpty(getCardRequest.Tag))
            {
                cardsIQueryable = cardsIQueryable.Where(m => m.Tag.Contains(getCardRequest.Tag));
            }

            int totalItemCount = cardsIQueryable.Count();

            List<CardModel> cardModelList = await cardsIQueryable
                .Skip((getCardRequest.PageNumber - 1) * getCardRequest.PageSize)
                .Take(getCardRequest.PageSize)
                .ToListAsync();

            if (cardModelList.Count == 0)
            {
                throw new NotFoundException(Resource.CardCouldNotFound);
            }

            return new CardListResponse
            {
                CardResponseList = _mapper.Map<List<CardResponse>>(cardModelList),
                TotalItemCount = totalItemCount
            };
        }

        public async Task DeleteCard(string userKey, long id)
        {
            _userAuthorizationService.CheckUserPermission(userKey);
            long userId = _userAuthorizationService.ConvertUserKeyToUserResponse(userKey).Id;

            var cardModel = await _cardRepository.GetItem(m => m.UserId == userId && m.Id == id);

            if (cardModel == null)
            {
                throw new NotFoundException(Resource.CardCouldNotFound);
            }

            cardModel.IsDeleted = true;
            cardModel.DeleteDate = DateTime.UtcNow;

            await _cardRepository.Update(cardModel);
        }
    }
}
