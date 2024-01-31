using Sales.Application.Interfaces;
using Sales.Core.Extensions;
using Sales.Domain.Common;
using Sales.Domain.Entities;
using Sales.Domain.Errors;
using Sales.Infra.Data.Interfaces;

namespace Sales.Application.Services;

public class AuthService(IUserRepository userRepository) : IAuthService
{

    private readonly IUserRepository _userRepository = userRepository;


    public async Task<Result<User>> SignIn(string email, string password)
    {
        try {
            var user = await _userRepository.SignInUser(email, password);
            return user; 
        }
        catch(Exception ex)
        {
            var error = new {
                message = ex.Message
            };
            return new BadRequestError(ex.Message, JsonSerializerExtensions.Serialize(error));
        }
    }
}
