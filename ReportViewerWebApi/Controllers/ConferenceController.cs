using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportViewerWebApi.Services;
using ReportViewerWebApi.ViewModels.ConferenceEditor;
using System.Net;
using System.Security.Claims;

namespace ReportViewerWebApi.Controllers;

[ApiController]
[Route("api/")]
public class ConferenceController : ControllerBase
{
    private readonly СonferenceService conferenceService;

    public ConferenceController(СonferenceService conferenceService)
    {
        this.conferenceService = conferenceService;
    }

    /// <summary>
    /// Get list of all conferences.
    /// </summary>
    /// <returns>Returns list of <see cref="ConferenceEditorViewModel"/></returns>
    [HttpGet]
    [Route("conferences")]
    [ProducesResponseType(typeof(IList<ConferenceEditorViewModel>), 200)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetAllConferences()
    {
        (HttpStatusCode code, IList<ConferenceEditorViewModel>? value) = await conferenceService.GetAllConferences();
        return ResolveCode(code, value);
    }

    /// <summary>
    /// Get a conference by id.
    /// </summary>
    /// <param name="conferenceId">An id of a conference.</param>
    /// <returns>Returns <see cref="ConferenceEditorViewModel"/></returns>
    [HttpGet]
    [Route("conference")]
    [ProducesResponseType(typeof(ConferenceEditorViewModel), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetConferenceById(string conferenceId)
    {
        (HttpStatusCode code, ConferenceEditorViewModel? value) = await conferenceService.GetConferenceById(conferenceId);
        return ResolveCode(code, value);
    }

    /// <summary>
    /// Create a new conference.
    /// </summary>
    /// <param name="conf">Conference object.</param>
    /// <returns>Returns newly created conference <see cref="ConferenceEditorViewModel"/></returns>
    [HttpPost]
    [Route("conference")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> CreateNewConference(ConferenceViewModel conf)
    {
        (HttpStatusCode code, ConferenceEditorViewModel? value)
            = await conferenceService.CreateConference(conf, User.FindFirstValue(ClaimTypes.Email));
        return ResolveCode(code, value);
    }

    /// <summary>
    /// Edit a conference.
    /// </summary>
    /// <param name="conf">Conference object.</param>
    /// <returns>Returns edited conference <see cref="ConferenceEditorViewModel"/></returns>
    [HttpPut]
    [Route("conference")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateConference(ConferenceEditorViewModel conf)
    {
        (HttpStatusCode code, ConferenceEditorViewModel? value)
            = await conferenceService.UpdateConference(conf, User.FindFirstValue(ClaimTypes.Email));
        return ResolveCode(code, value);
    }

    /// <summary>
    /// Delete a conference.
    /// </summary>
    /// <param name="conferenceId">An id of a conference.</param>
    /// <returns>Returns ID of a conference if success, otherwise <see cref="HttpStatusCode.BadRequest"/></returns>
    [HttpDelete]
    [Route("conference")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteConference(string conferenceId)
    {
        (HttpStatusCode code, string? value) = await conferenceService.DeleteConference(conferenceId);
        return ResolveCode(code, value);
    }

    /// <summary>
    /// Converts <see cref="HttpStatusCode"/> to object result.
    /// </summary>
    private IActionResult ResolveCode(HttpStatusCode httpCode, object? value) =>
        httpCode switch
        {
            HttpStatusCode.NotFound => NotFound(value),
            HttpStatusCode.BadRequest => BadRequest(value),
            HttpStatusCode.OK => Ok(value),
            _ => BadRequest(value),
        };
}
