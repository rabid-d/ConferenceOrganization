using Microsoft.AspNetCore.Mvc;
using ReportViewerWebApi.Services;
using ReportViewerWebApi.ViewModels;

namespace ReportViewerWebApi.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService userService;

    public UserController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpPost]
    [Route("api/registration")]
    public async Task<IActionResult> RegisterUser(UserViewModel user)
    {
        //await userService.RegisterUser(user);
        return Ok();
    }

    [HttpPost]
    [Route("api/login")]
    public IActionResult LoginUser(UserViewModel user)
    {
        return Ok();
    }
}
