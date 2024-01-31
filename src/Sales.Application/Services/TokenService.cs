using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson;
using Sales.Application.Interfaces;
using Sales.Application.JWT;

namespace Sales.Application.Services;

//TODO: Migrar toda essa lógica para um Redis.

public class TokenService(ITokenGenerator _tokenGenerator) : ITokenService
{

    private readonly Dictionary<string, JwtSecurityToken> _userTokens = [];
    private readonly Dictionary<JwtSecurityToken, string> _tokenUsers = [];

    public void SetTokenForUser(string userId, JwtSecurityToken token)
    {
        _userTokens[userId] = token;
        _tokenUsers[token] = userId;
    }

    public JwtSecurityToken GetTokenForUser(string userEmail, string userId)
    {
        return _userTokens.TryGetValue(userId, out var token) ? token : _tokenGenerator.GenerateToken(userEmail, userId);
    }


    public string GetUserIdFromToken(string token)
    {
        if (_tokenUsers.TryGetValue(new JwtSecurityToken(token), out var userId))
        {
            return userId;
        }
        return "";
    }
}
