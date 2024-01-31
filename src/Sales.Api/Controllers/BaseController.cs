
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson;


[Controller]
public class BaseController : ControllerBase
{
    protected JwtSecurityToken? Jwt { get; set; } = null;

    protected string UserId { get; set; }

    protected JwtSecurityTokenHandler Handler;

    public BaseController(IHttpContextAccessor httpContextAccessor)
    {
        Handler = new JwtSecurityTokenHandler();
        var context = httpContextAccessor.HttpContext;
        var token = context
        .Request
        .Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

        if(token != string.Empty) {
            Jwt = Handler.ReadJwtToken(token);
            UserId = new ObjectId(Jwt?.Claims.FirstOrDefault(claim => claim.Type == "id")?.Value).ToString();
        } 

    }
}
