using Microsoft.AspNetCore.Mvc;
using Sales.Application.Interfaces;
using Sales.Core.DTOs;
using Sales.Core.Helpers;

namespace Sales.Api.Controllers;

[ApiController]
[Route("api/notify")]
public class NotifierController : BaseController
{

    private readonly INotifierService _notifierService;

    private readonly IProjectService _projectService;

    private readonly ITokenService _tokenService;

    public NotifierController(
        INotifierService notifierService,
        IProjectService projectService,
        ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _notifierService = notifierService;
        _projectService = projectService;
        _tokenService = tokenService;
    }

    [HttpPost("count/{sessionId}")]
    public async Task<IActionResult> UpdateCountAsync(int sessionId, [FromBody] NotifierDTO notifierDTO)
    {
        notifierDTO.SessionId = sessionId;
        var currentProject = await _projectService.GetProjectFileBySessionIdAsync(sessionId);
        var jwt =  _tokenService.GetTokenForUser(currentProject.UserEmail,  currentProject.UserId).RawData;
        notifierDTO.UserId = currentProject.UserId;
        notifierDTO.Token = jwt;
        var result = await _notifierService.UpdateFileStatus(notifierDTO);

        return result.Match(
           response =>
           {
               return Ok(notifierDTO);
           },
           error => error.ErrorToActionResult()
       );
       

    }

    [HttpPost("extract")]
    public async Task<IActionResult> UpdateExtractAsync([FromBody] NotifierDTO notifierDTO)
    {
        var currentProject = await _projectService.GetProjectFileBySessionIdAsync(notifierDTO.SessionId);
        var jwt =  _tokenService.GetTokenForUser(currentProject.UserEmail,  currentProject.UserId).RawData;
        notifierDTO.UserId = currentProject.UserId;
        notifierDTO.Token = jwt;
        notifierDTO.UserEmail = currentProject.UserEmail;
        var result = await _notifierService.UpdateExtractionStatus(notifierDTO);

        return result.Match(
           response =>
           {
               return Ok(notifierDTO);
           },
           error => error.ErrorToActionResult()
       );
       

    }
}
