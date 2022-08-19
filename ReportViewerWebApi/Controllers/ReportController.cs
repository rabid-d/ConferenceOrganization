using Microsoft.AspNetCore.Mvc;
using ReportViewerWebApi.Services;
using ReportViewerWebApi.ViewModels.ConferenceEquipment;
using ReportViewerWebApi.ViewModels.ConferencePersons;
using ReportViewerWebApi.ViewModels.ConferenceSchedule;

namespace ReportViewerWebApi.Controllers;

/// <summary>
/// Getting reports of conferences.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly СonferenceService conferenceService;

    /// <summary>
    /// Default value of action parameter in swagger.
    /// </summary>
    private const string defaultConferenceName = "Conference 2. A New Method of Cockroach Control on Submarines";

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="conferenceService">Service for work with conferences.</param>
    public ReportController(СonferenceService conferenceService)
    {
        this.conferenceService = conferenceService;
    }

    /// <summary>
    /// Get shedule of a conference.
    /// </summary>
    /// <param name="conferenceName">Name of a conference.</param>
    /// <returns>Detailed schedule of a conference.</returns>
    [HttpGet]
    [Route("schedule")]
    [ProducesResponseType(typeof(ConferenceScheduleViewModel), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetConferenceScheduleByName(string conferenceName = defaultConferenceName)
    {
        ConferenceScheduleViewModel? conference = await conferenceService.GetConferenceSchedule(conferenceName);
        if (conference == null)
        {
            return NotFound();
        }

        return Ok(conference);
    }

    /// <summary>
    /// Get all the people involved in the conference.
    /// </summary>
    /// <param name="conferenceName">Name of a conference.</param>
    /// <returns>List of chairpersons of sections and speakers of talks.</returns>
    [HttpGet]
    [Route("users")]
    [ProducesResponseType(typeof(ConferenceUsersViewModel), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetConferenceUsersByName(string conferenceName = defaultConferenceName)
    {
        ConferenceUsersViewModel? confUsers = await conferenceService.GetPersonsOfConference(conferenceName);
        if (confUsers == null)
        {
            return NotFound();
        }

        return Ok(confUsers);
    }

    /// <summary>
    /// Get all the equipment that will be needed in a conference.
    /// </summary>
    /// <param name="conferenceName">Name of a conference.</param>
    /// <returns>List of equipment and information in what room and during what time it will be busy.</returns>
    [HttpGet]
    [Route("equipment")]
    [ProducesResponseType(typeof(ConferenceEquipmentViewModel), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetConferenceEquipmentByName(string conferenceName = defaultConferenceName)
    {
        ConferenceEquipmentViewModel? confUsers = await conferenceService.GetConferenceEquipmentByName(conferenceName);
        if (confUsers == null)
        {
            return NotFound();
        }

        return Ok(confUsers);
    }
}
