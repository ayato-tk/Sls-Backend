using Microsoft.AspNetCore.Mvc;
using Sales.Application.Interfaces;
using AutoMapper;
using Sales.Core.DTOs;
using Sales.Core.Helpers;
using Sales.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Sales.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/project")]
public class ProjectController(
IProjectService _projectService, 
IMapper _mapper, 
ITokenService _tokenService,
IHttpContextAccessor httpContextAccessor
) : BaseController(httpContextAccessor)
{

    [HttpPost("custom")]
    public async Task<IActionResult> PostCustomProjectAsync([FromBody] ProjectDTO projectDto)
    {
        _tokenService.SetTokenForUser(projectDto.UserId, Jwt);

        var result = await _projectService.CreateCustomProjectAsync(projectDto);

        return result.Match(
            response => Ok(_mapper.Map<CustomProjectViewModel>(response)),
            error => error.ErrorToActionResult()
        );

    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjectsAsync()
    {
        var result = await _projectService.GetAllProjectsAsync(UserId);

        return result.Match(
            response => Ok(result),
            error => error.ErrorToActionResult()
        );
    }  
}