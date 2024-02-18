using CRUDBooks.Data;
using CRUDBooks.Models;
using Microsoft.AspNetCore.Mvc;
using CRUDBooks.Services.ServiceInterfaces;

namespace CRUDBooks.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ITokenService tokenService;
        private readonly IAuthService authService;
        private readonly IRegistrationService registrationService;

        public AccountController(ITokenService tokenService, IAuthService authService, IRegistrationService registrationService)
        {
            this.tokenService = tokenService;
            this.authService = authService;
            this.registrationService = registrationService;
        }

        /// <summary>
        /// Регистрация в системе (добавление нового пользователя)
        /// </summary>
        /// 
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /login
        ///    {
        ///     "login": "Someone",
        ///     "password": "111"
        ///    }
        /// 
        /// </remarks>
        [HttpPost("/registration")]
        public async Task<IActionResult> Registration([FromBody]User user)
        {
            if (user == null) { return BadRequest(); }   //Не удалось десериализовать объект User

            if (registrationService.RegisterUser(user)) { return Ok(); }

            return BadRequest();
        }

        /// <summary>
        /// Аутентификация в системе
        /// </summary>
        /// 
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /login
        ///    {
        ///     "login": "Konstantin",
        ///     "password": "12345"
        ///    }
        /// 
        /// </remarks>
        [HttpPost("/login")]
        public async Task<IActionResult> LoginAuthentication([FromBody]User user)
        {
            if(authService.VerifyUser(user)) { return Content(tokenService.GenerateToken(user.Login)); }
            
            return Unauthorized();
        }

    }
}
