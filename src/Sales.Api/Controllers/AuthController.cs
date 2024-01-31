using Microsoft.AspNetCore.Mvc;
using Sales.Api.Models;
using Sales.Application.Interfaces;
using Sales.Application.JWT;
using Sales.Core.Helpers;

namespace Sales.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(
    ITokenGenerator tokenGenerator,
    ITokenService tokenService, 
    IAuthService authService) : ControllerBase
    {

        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;

        private readonly IAuthService _authService = authService;

        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] AuthModel authModel)
        {
            var result = await _authService.SignIn(authModel.Email, authModel.Password);
            
            
            return result.Match(
                response => {
                    var xtoken = _tokenGenerator.GenerateToken(authModel.Email, response.Id);
                    _tokenService.SetTokenForUser(response.Id, xtoken);
                    return Ok( new { xtoken = xtoken.RawData });
                },
                error => error.ErrorToActionResult()
            );
        }
    }
}

