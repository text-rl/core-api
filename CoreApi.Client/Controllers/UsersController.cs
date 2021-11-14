using System;
using System.Threading.Tasks;
using Application.Write.Users.LoginUser;
using CoreApi.ApplicationCore.Write.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("[action]", Name = nameof(Registration))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registration(RegisterUserCommand registerUserCommand)
        {
            try
            {
                var res = await _sender.Send(registerUserCommand);
                return CreatedAtRoute(nameof(Authentication), new { });
            }
            catch (ApplicationException e)
            {
                return Conflict();
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]", Name = nameof(Authentication))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Authentication(LoginUserQuery loginUserQuery)
        {
            try
            {
                TokenResponse? userToken = await _sender.Send(loginUserQuery);
                return Ok(userToken);
            }
            catch (ApplicationException e)
            {
                return NotFound();
            }
        }
    }
}