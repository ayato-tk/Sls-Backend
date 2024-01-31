using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson;

namespace Sales.Application.JWT;

public interface ITokenGenerator
{
    JwtSecurityToken GenerateToken(string email, string id);
}
