using DocumentRepositoryApi.Models;
using DocumentRepositoryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IUserService _userService;
        public AuthController(IAuthService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> Generate([BindRequired, FromBody]Login loginModel)
        {
            var token = await _service.GenerateToken(loginModel.Email, loginModel.Password);
            if (string.IsNullOrEmpty(token))
                return BadRequest();

            return Ok(token);
        }

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
