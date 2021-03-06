using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Contracts.Requests.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace LearnAndRepeatWeb.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("")]
        public async Task<IActionResult> PostUser(PostUserRequest postUserRequest)
        {
            var result = await _userService.PostUser(postUserRequest);

            return StatusCode((int)HttpStatusCode.OK, result);
        }

        [HttpPost("authentication-token")]
        public async Task<IActionResult> PostToken([FromBody] PostAuthenticationTokenRequest postAuthenticationTokenRequest)
        {
            var result = await _userService.PostAuthenticationToken(postAuthenticationTokenRequest);

            return StatusCode((int)HttpStatusCode.OK, result);
        }

        [HttpPut("{id}/verify/{confirmationToken}")] 
        public async Task<IActionResult> PutUserAsVerified([FromRoute] long id, [FromRoute] string confirmationToken)
        {
            await _userService.PutUserAsConfirmed(id, confirmationToken);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPatch("{userKey}/update")]
        public async Task<IActionResult> Patch([FromRoute] string userKey, [FromBody] PatchUserRequest patchUserRequest)
        {
            await _userService.PatchUser(userKey, patchUserRequest);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [Authorize]
        [HttpGet("{userKey}")]
        public async Task<IActionResult> Get([FromRoute] string userKey)
        {
            var result = await _userService.GetUser(userKey);

            return StatusCode((int)HttpStatusCode.OK, result);
        }
    }
}
