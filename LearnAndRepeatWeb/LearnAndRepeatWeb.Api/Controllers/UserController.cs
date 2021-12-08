﻿using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Contracts.Requests.User;
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

        [HttpPost("token")]
        public IActionResult PostToken([FromBody] PostTokenRequest postTokenRequest)
        {
            var result = _userService.PostToken(postTokenRequest);

            return StatusCode((int)HttpStatusCode.OK, result);
        }
    }
}
