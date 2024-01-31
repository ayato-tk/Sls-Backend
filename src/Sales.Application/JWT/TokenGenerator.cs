using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace Sales.Application.JWT;

public class TokenGenerator(IConfiguration configuration) : ITokenGenerator
{

    private readonly IConfiguration _configuration = configuration;

    /* TODO: 
        Trocar a SecretKey da api para mais de 256 bytes e migrar a Microsoft.AspNetCore.Authentication.JwtBearer
        para as versões mais atuais.
    */

    public JwtSecurityToken GenerateToken(string email, string id)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(
            _configuration["JwtOptions:SecretKey"]
            ??
            throw new Exception("SecretKey is not defined.")
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new("id", id),
                    new(ClaimTypes.Email, email),
                }
            ),
            Expires = DateTime.UtcNow.AddHours(
                int.Parse(
                    _configuration["JwtOptions:Expires"]
                    ??
                    throw new Exception("Expires time is not defined.")
                )
                ),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new JwtSecurityToken(tokenHandler.WriteToken(token));
    }
}
