using IdentityVSAPI.Domain.Entity.Dto;
using IdentityVSAPI.Domain.Response;
using IdentityVSAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityVSAPI.Controllers
{
    [Route("api/accounts")]
    [Produces("application/json")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IAccountServicesAuthAsync _accountSer;
        private readonly ILogger<AccountsController> _logger;
        public AccountsController(IAccountServicesAuthAsync accountSer, ILogger<AccountsController> logger)
        {
            _accountSer = accountSer;
            _logger = logger;
        }
        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Authenticate user.</returns>
        /// <remarks>
        /// Образец запроса:
        /// 
        ///     POST /login        
        ///     
        /// </remarks>
        /// <response code="200"> Запрос прошёл. (Успех) </response>
        /// <response code="400"> Недопустимое значение ввода </response>
        /// <response code="401"> Пользователь не авторизован. </response>
        /// <response code="406"> Несоответствующие данные. </response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthenticationDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AuthResponse authResponse = await _accountSer.AuthenticateAsync(request);

            if (authResponse.Status is Status.NotFound)
                return NotFound(authResponse);

            if (authResponse.Status is Status.PasswordInvalid)
                return new ObjectResult(authResponse) { StatusCode = StatusCodes.Status406NotAcceptable };

            if (authResponse.Status is Status.Unauthorized) 
                return Unauthorized();

            return Ok(authResponse);
        }
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Register user</returns>
        /// <remarks>
        /// Образец запроса:
        /// 
        ///     POST /register        
        ///     
        /// </remarks>
        /// <response code="200"> Запрос прошёл. (Успех) </response>
        /// <response code="400"> Пользователь не найден. </response>
        /// <response code="406"> Несоответствующие данные. </response>
        /// <response code="422"> Не зарегистрирован. </response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid) 
                return BadRequest(request);

            AuthResponse authResponse = await _accountSer.RegisterAsync(request);

            if (authResponse.Status is Status.NotFound)
                return NotFound(authResponse);

            if (authResponse.Status is Status.Exists || authResponse.Status is Status.Mismatch)
                return new ObjectResult(authResponse) { StatusCode = StatusCodes.Status406NotAcceptable };

            if (authResponse.Status is Status.NotCreated)
                return new ObjectResult(authResponse) { StatusCode = StatusCodes.Status422UnprocessableEntity };

            return Ok(authResponse);
            //return await Authenticate(new AuthenticationDto
            //{
            //    Email = request.Email,
            //    Password = request.Password
            //});
        }
    }
}