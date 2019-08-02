using DocumentRepositoryApi.Models;
using DocumentRepositoryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Controllers
{

    /// <summary>
    /// Auth Controller to generate token and register user
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IUserService _userService;

        public AuthController(IAuthService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        /// <summary>
        /// Generates token for Authorization header
        /// </summary>
        /// <remarks></remarks>
        /// <param name="loginModel"></param>
        /// <returns>A newly created token</returns>
        /// <response code="200">Returns the newly created token</response>
        /// <response code="400">If the credential is not granted</response>  
        /// <response code="500">If something goes wrong</response>  
        [HttpPost("token")]
        public async Task<IActionResult> Generate([BindRequired, FromBody]Login loginModel)
        {
            var token = await _service.GenerateToken(loginModel.Email, loginModel.Password);
            if (string.IsNullOrEmpty(token))
                return BadRequest();

            return Ok(token);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <remarks></remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <response code="200">Successfuly created user</response>
        /// <response code="500">If something goes wrong</response>  
        [HttpPost("register")]
        public async Task<IActionResult> Register([BindRequired, FromBody]User user)
        {
            var result = await _userService.Register(user);
            if (result)
                return Ok();

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
