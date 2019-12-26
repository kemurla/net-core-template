using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using CAT.Core.Models;
using CAT.Core.Abstractions.Services;

namespace CAT.Web.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a user.
        /// </summary>
        /// <param name="registerModel">The model that represents the user data.</param>
        /// <returns>An error message if creating a user failed.</returns>
        /// <response code="200">The user was created successfully.</response>
        /// <response code="400">Invalid input.</response>
        [HttpPost("register")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody]RegisterModel registerModel)
        {
            var registerResult = await _userService.RegisterAsync(registerModel);

            if (registerResult.Success)
                return Ok();

            return BadRequest(registerResult.Message);
        }

        /// <summary>
        /// User login.
        /// </summary>
        /// <param name="loginModel">The model that represents the user credentials.</param>
        /// <returns>A JWT token.</returns>
        /// <response code="200">Correct credentials.</response>
        /// <response code="400">Invalid credentials or don't match criteria.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(JwtToken), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody]LoginModel loginModel)
        {
            var loginResult = await _userService.LoginAsync(loginModel);

            if (loginResult.Success)
                return Ok(loginResult.Data);

            return BadRequest(loginResult.Message);
        }
    }
}