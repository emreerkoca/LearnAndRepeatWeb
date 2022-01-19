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

            return StatusCode((int)HttpStatusCode.Created, result); 
        }

        [HttpPatch("{userKey}")]
        public async Task<IActionResult> Patch([FromRoute] string userKey, [FromBody] PatchCardRequest patchCardRequest)
        {
            await _cardService.PatchCard(userKey, patchCardRequest);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpGet("{userKey}")]
        public async Task<IActionResult> Get([FromRoute] string userKey, [FromQuery] GetCardRequest getCardRequest)
        {
            var result = await _cardService.GetCard(userKey, getCardRequest);

            return StatusCode((int)HttpStatusCode.OK, result);
        }

        [HttpDelete("{userKey}/delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string userKey, [FromRoute] long id)
        {
            await _cardService.DeleteCard(userKey, id);

            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}
