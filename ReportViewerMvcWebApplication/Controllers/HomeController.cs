using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReportViewerMvcWebApplication.Models;
using ReportViewerMvcWebApplication.Resources;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ReportViewerMvcWebApplication.Controllers;

public class HomeController : Controller
{
    private readonly Repository repository;
    private readonly IHttpContextAccessor contextAccessor;
    private readonly ConfigurationManager configuration;

    public HomeController(Repository repository, IHttpContextAccessor IHttpContextAccessor, ConfigurationManager configuration)
    {
        this.repository = repository;
        this.contextAccessor = IHttpContextAccessor;
        this.configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        IList<string> conferencesNames = await repository.GetConferencesNames();
        return View(conferencesNames);
    }

    [HttpPost]
    public async Task<IActionResult> ShowReport(string reportType, string conferenceName)
    {
        IList<string> reportValues = new List<string>();
        ReportType reportTypeEnum = (ReportType)Enum.Parse(typeof(ReportType), reportType);
        switch (reportTypeEnum)
        {
            case ReportType.Schedule:
                ViewData[Resource.Title] = Resource.ConferenceScheduleViewTitle;
                reportValues = await repository.GetConfSchedule(conferenceName);
                break;
            case ReportType.Participants:
                ViewData[Resource.Title] = Resource.ConferenceParticipantsViewTitle;
                reportValues = await repository.GetConfParticipants(conferenceName);
                break;
            case ReportType.Equipment:
                ViewData[Resource.Title] = Resource.ConferenceEquipmentViewTitle;
                reportValues = await repository.GetConfEquipment(conferenceName);
                break;
            default:
                break;
        }
        return View(Resource.Report, reportValues);
    }

    public IActionResult NewUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> NewUser(string name, string degree, string work, string positon, string bio, IFormFile photo)
    {
        await repository.AddUser(name, degree, work, positon, bio, photo);
        return View(Resource.UserAdded);
    }

    public async Task<IActionResult> Conferences()
    {
        return View(await repository.GetAllConferences());
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateConference(Conference conference)
    {
        if (ModelState.IsValid)
        {
            await repository.AddConference(conference);
        }
        //var errors = ModelState.Values.SelectMany(v => v.Errors);

        return Json(true);
    }

    [AcceptVerbs("GET", "POST")]                                               // ⬇
    public IActionResult VerifyConferenceStartDate([Bind(Prefix = "Conference.DateStart")] DateTime dateTime)
    {
        return VerifyStartAndDate(dateTime);
    }

    [AcceptVerbs("GET", "POST")]                                             // ⬇
    public IActionResult VerifyConferenceEndDate([Bind(Prefix = "Conference.DateEnd")] DateTime dateTime)
    {
        return VerifyStartAndDate(dateTime);
    }

    private IActionResult VerifyStartAndDate(DateTime dateTime)
    {
        if ((dateTime.DayOfWeek == DayOfWeek.Saturday) || (dateTime.DayOfWeek == DayOfWeek.Sunday))
        {
            return Json(Resource.WeeekendSelectedError);
        }
        return Json(true);
    }

    public async Task<IActionResult> ListOfAllConferences()
    {
        return PartialView("Partial/_ListOfAllConferences", await repository.GetAllConferences());
    }

    [HttpGet]
    public async Task<IActionResult> EditConference(string id)
    {
        DAL.Model.Conference? conference = await repository.GetConferenceById(id);
        Conference conf = new() { Name = conference.Name, Address = conference.Address, DateStart = conference.DateStart, DateEnd = conference.DateEnd };
        return PartialView(Resource.EditConference, conf);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> EditConference(Conference conf, string id)
    {
        if (ModelState.IsValid)
        {
            await repository.UpateConferende(conf, id);
            return Json(Resource.Valid);
        }
        else
        {
            return Json(Resource.NotValid);
        }
    }

    [Authorize]
    public async Task<IActionResult> DeleteConference(string id)
    {
        await repository.DeleteConference(id);
        return Json(id);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(AppUser appUser)
    {
        if (!await repository.IsLoginCredentialsValid(appUser))
        {
            return BadRequest();
        }

        string token = CreateToken(appUser);
        contextAccessor.HttpContext?.Response.Cookies.Append(Resource.JwtToken, token, new CookieOptions { HttpOnly = true });

        return RedirectToAction("Conferences", await repository.GetAllConferences());
    }

    private string CreateToken(AppUser appUser)
    {
        byte[] key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JWT:Key"));
        JwtSecurityToken jwtToken = new(
            issuer: configuration.GetValue<string>("JWT:Issuer"),
            audience: configuration.GetValue<string>("JWT:Audience"),
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Registration(AppUser appUser)
    {
        if (ModelState.IsValid)
        {
            await repository.AddAppUser(appUser);
            return RedirectToAction("Login");
        }
        return BadRequest();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        foreach (var cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }

        return RedirectToAction("Conferences", await repository.GetAllConferences());
    }
}
