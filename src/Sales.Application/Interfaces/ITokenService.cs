using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson;

namespace Sales.Application.Interfaces;

public interface ITokenService
{
    void SetTokenForUser(string userId, JwtSecurityToken token);

    JwtSecurityToken GetTokenForUser(string email, string userId);
}
