using LearnAndRepeatWeb.Contracts.Requests.Card;
using LearnAndRepeatWeb.Contracts.Responses.Card;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Business.Services.Interfaces
{
    public interface ICardService
    {
        Task<CardResponse> PostCard(string userKey, PostCardRequest postCardRequest);
    }
}
