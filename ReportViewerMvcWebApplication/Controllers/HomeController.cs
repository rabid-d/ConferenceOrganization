using Microsoft.AspNetCore.Mvc;
using ReportViewerMvcWebApplication.Models;
using System.Diagnostics;

namespace ReportViewerMvcWebApplication.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Repository repository;

    public HomeController(ILogger<HomeController> logger, Repository repository)
    {
        _logger = logger;
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
        if (reportType == "schedule")
        {
            ViewData["Title"] = "Conference Schedule";
            reportValues = await repository.GetConfSchedule(conferenceName);
        }
        else if (reportType == "participants")
        {
            ViewData["Title"] = "List of participants";
            reportValues = await repository.GetConfParticipants(conferenceName);
        }
        else if (reportType == "equipment")
        {
            ViewData["Title"] = "List of equipment";
            reportValues = await repository.GetConfEquipment(conferenceName);
        }
        return View("Report", reportValues);
    }

    public IActionResult NewUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> NewUser(string name, string degree, string work, string positon, string bio, IFormFile photo)
    {
        await repository.AddUser(name, degree, work, positon, bio, photo);
        return View("UserAdded");
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
