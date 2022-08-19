using Microsoft.AspNetCore.Mvc;
using ReportViewerWebApi.Services;
using ReportViewerWebApi.ViewModels;
using System.Net;

namespace ReportViewerWebApi.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService userService;

    public UserController(UserService userService)
    {
        this.userService = userService;
    }

    /// <summary>
    /// Register new user.
    /// </summary>
    /// <param name="user">User that will be created.</param>
    /// <returns>Returns <see cref="UserViewModel"/> or <see cref="HttpStatusCode.BadRequest"/></returns>
    [HttpPost]
    [Route("api/registration")]
    [ProducesResponseType(typeof(UserViewModel), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RegisterUser(UserViewModel user)
    {
        if (await userService.RegisterUser(user))
        {
            return Created("", user);
        }
        else
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Login with credentials.
    /// </summary>
    /// <param name="user">Credentials.</param>
    /// <returns>Returns «JSON Web Token» or <see cref="HttpStatusCode.NotFound"/></returns>
    [HttpPost]
    [Route("api/login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> LoginUser(UserViewModel user)
    {
        string? token = await userService.Login(user);
        if (token == null)
        {
            return NotFound();
        }
        return Ok(token);
    }
}
