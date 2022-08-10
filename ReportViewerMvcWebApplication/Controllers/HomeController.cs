using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportViewerMvcWebApplication.Models;
using ReportViewerMvcWebApplication.Resources;
using System.Diagnostics;

namespace ReportViewerMvcWebApplication.Controllers;

public class HomeController : Controller
{
    private readonly Repository repository;

    public HomeController(Repository repository)
    {
        this.repository = repository;
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
        return PartialView(await repository.GetAllConferences());
    }

    [HttpGet]
    public async Task<IActionResult> EditConference(string id)
    {
        DAL.Model.Conference? conference = await repository.GetConferenceById(id);
        Conference conf = new() { Name = conference.Name, Address = conference.Address, DateStart = conference.DateStart, DateEnd = conference.DateEnd };
        return PartialView(Resource.EditConference, conf);
    }

    [HttpPost]
    public async Task<IActionResult> EditConference(Conference conf, string id)
    {
        if (ModelState.IsValid)
        {
            await repository.UpateConferende(conf, id);
            return Json(Resource.Valid);
        } else
        {
            return Json(Resource.NotValid);
        }
    }

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
}
