using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Contracts.Requests.Card;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost("{userKey}")]
        public async Task<IActionResult> Post([FromRoute] string userKey, [FromBody] PostCardRequest postCardRequest)
        {
            var result = await _cardService.PostCard(userKey, postCardRequest);

            return StatusCode((int)HttpStatusCode.OK, result); 
        }
    }
}
