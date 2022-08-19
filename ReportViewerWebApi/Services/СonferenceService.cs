using AutoMapper;
using ReportViewerWebApi.ViewModels.ConferenceEditor;
using ReportViewerWebApi.ViewModels.ConferenceEquipment;
using ReportViewerWebApi.ViewModels.ConferencePersons;
using ReportViewerWebApi.ViewModels.ConferenceSchedule;
using System.Net;

namespace ReportViewerWebApi.Services;

/// <summary>
/// Provides access to information about conferences.
/// </summary>
public class СonferenceService
{
    private readonly DAL.Services.ConferenceService conferenceService;
    private readonly UserService userService;
    private readonly IMapper mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="conferenceService">DAL service to work with conferences.</param>
    /// <param name="mapper"></param>
    public СonferenceService(DAL.Services.ConferenceService conferenceService, UserService userService, IMapper mapper)
    {
        this.conferenceService = conferenceService;
        this.userService = userService;
        this.mapper = mapper;
    }

    /// <summary>
    /// Get shedule of a conference.
    /// </summary>
    /// <param name="conferenceName">Name of a conference.</param>
    /// <returns>Conference with its sections and talks.</returns>
    public async Task<ConferenceScheduleViewModel?> GetConferenceSchedule(string conferenceName)
    {
        DAL.Model.Conference? conference = await GetConferenceByName(conferenceName);
        if (conference == null)
        {
            return null;
        }
        ConferenceScheduleViewModel resultConf = new();
        resultConf.ConferenceName = conference.Name;
        resultConf.DateStart = conference.DateStart;
        resultConf.DateEnd = conference.DateEnd;
        resultConf.Sections = new();
        foreach (DAL.Model.Section section in conference.Sections)
        {
            SectionScheduleViewModel newSection = new();
            newSection.Name = section.Name;
            newSection.Chairperson = section.Chairperson.FullName;
            newSection.Room = section.Room;
            newSection.Talks = new();
            foreach (DAL.Model.Talk talk in section.Talks)
            {
                TalkScheduleViewModel newTalk = new();
                newTalk.Theme = talk.Theme;
                newTalk.Speaker = talk.Speaker.FullName;
                newTalk.DateStart = talk.DateStart;
                newTalk.DateEnd = talk.DateEnd;
                newSection.Talks.Add(newTalk);
            }
            resultConf.Sections.Add(newSection);
        }

        return resultConf;
    }

    /// <summary>
    /// Get chairpersons and skeakers of a conference.
    /// </summary>
    /// <param name="conferenceName">Name of a conference.</param>
    /// <returns>List of chairpersons and speakers.</returns>
    public async Task<ConferenceUsersViewModel?> GetPersonsOfConference(string conferenceName)
    {
        DAL.Model.Conference? conference = await GetConferenceByName(conferenceName);
        if (conference == null)
        {
            return null;
        }
        ConferenceUsersViewModel confUsers = new();
        confUsers.ConferenceName = conferenceName;
        foreach (DAL.Model.Section section in conference.Sections)
        {
            ConferencePersonViewModel person = new();
            person.Fullname = section.Chairperson.FullName;
            person.Work = section.Chairperson.Work;
            person.Position = section.Chairperson.Position;
            confUsers.Chairpersons.Add(person);
        }
        foreach (DAL.Model.Section section in conference.Sections)
        {
            foreach (DAL.Model.Talk talk in section.Talks)
            {
                ConferencePersonViewModel person = new();
                person.Fullname = talk.Speaker.FullName;
                person.Work = talk.Speaker.Work;
                person.Position = talk.Speaker.Position;
                confUsers.Speakers.Add(person);
            }
        }

        return confUsers;
    }

    /// <summary>
    /// Get equipment needed for conference and time when it will be busy.
    /// </summary>
    /// <param name="conferenceName">Name of a conference.</param>
    /// <returns>List of equipment, room and time when and where it will be used.</returns>
    public async Task<ConferenceEquipmentViewModel?> GetConferenceEquipmentByName(string conferenceName)
    {
        DAL.Model.Conference? conference = await GetConferenceByName(conferenceName);
        if (conference == null)
        {
            return null;
        }
        ConferenceEquipmentViewModel conferenceEquipment = new() { ConferenceName = conference.Name };
        HashSet<DAL.Model.Equipment> equipment = new();
        foreach (DAL.Model.Section section in conference.Sections)
        {
            foreach (DAL.Model.Talk talk in section.Talks)
            {
                foreach (DAL.Model.Equipment equip in talk.Equipment)
                {
                    equipment.Add(equip);
                }
            }
        }
        foreach (DAL.Model.Equipment equip in equipment)
        {
            foreach (DAL.Model.Section section in conference.Sections)
            {
                foreach (DAL.Model.Talk talk in section.Talks)
                {
                    if (talk.Equipment.Contains(equip))
                    {
                        BusyEquipmentViewModel busyEquip = new();
                        busyEquip.EquipmentName = equip.Name;
                        busyEquip.Room = section.Room;
                        busyEquip.DateStart = talk.DateStart;
                        busyEquip.DateEnd = talk.DateEnd;

                        conferenceEquipment.Equipment.Add(busyEquip);
                    }
                }
            }
        }

        return conferenceEquipment;
    }

    public async Task<Tuple<HttpStatusCode, IList<ConferenceEditorViewModel>?>> GetAllConferences()
    {
        List<DAL.Model.Conference> dalConferences = await conferenceService.GetAllConferences();
        IList<ConferenceEditorViewModel> conferences = mapper.Map<List<ConferenceEditorViewModel>>(dalConferences);
        if (conferences.Count == 0)
        {
            return Tuple.Create<HttpStatusCode, IList<ConferenceEditorViewModel>?>(HttpStatusCode.NoContent, null);
        }
        return Tuple.Create(HttpStatusCode.OK, conferences);
    }

    public async Task<Tuple<HttpStatusCode, ConferenceEditorViewModel?>> GetConferenceById(string conferenceId)
    {
        DAL.Model.Conference? dalConference = await conferenceService.GetConferenceById(conferenceId);
        if (dalConference == null)
        {
            return Tuple.Create<HttpStatusCode, ConferenceEditorViewModel?>(HttpStatusCode.NotFound, null);
        }
        return Tuple.Create(HttpStatusCode.OK, mapper.Map<ConferenceEditorViewModel>(dalConference));
    }

    public async Task<Tuple<HttpStatusCode, ConferenceEditorViewModel?>> CreateConference(ConferenceViewModel conf, string userEmail)
    {
        Guid? guid = await userService.GetUserGuid(userEmail);
        if (guid == null)
        {
            return Tuple.Create<HttpStatusCode, ConferenceEditorViewModel?>(HttpStatusCode.Unauthorized, null);
        }

        DAL.Model.Conference newConference = mapper.Map<DAL.Model.Conference>(conf);
        newConference.ConferenceId = Guid.NewGuid();
        newConference.CreatedBy = (Guid)guid;
        newConference.CreatedDate = DateTime.UtcNow;
        ConferenceEditorViewModel returnConference = mapper.Map<ConferenceEditorViewModel>(newConference);
        bool success = await conferenceService.AddNewConference(newConference);
        if (success)
        {
            return Tuple.Create<HttpStatusCode, ConferenceEditorViewModel?>(HttpStatusCode.OK, returnConference);
        }
        else
        {
            return Tuple.Create<HttpStatusCode, ConferenceEditorViewModel?>(HttpStatusCode.BadRequest, null);
        }
    }

    public async Task<Tuple<HttpStatusCode, ConferenceEditorViewModel?>> UpdateConference(ConferenceEditorViewModel conference, string userEmail)
    {
        Guid? guid = await userService.GetUserGuid(userEmail);
        if (guid == null)
        {
            return Tuple.Create<HttpStatusCode, ConferenceEditorViewModel?>(HttpStatusCode.Unauthorized, null);
        }

        if (await conferenceService.GetConferenceById(conference.ConferenceId.ToString()) == null)
        {
            return Tuple.Create<HttpStatusCode, ConferenceEditorViewModel?>(HttpStatusCode.NotFound, null);
        }
        DAL.Model.Conference dalConference = mapper.Map<DAL.Model.Conference>(conference);
        dalConference.ModifiedBy = guid;
        dalConference.ModifiedDate = DateTime.UtcNow;
        string id = conference.ConferenceId.ToString();
        bool success = await conferenceService.UpdateConference(dalConference, id);
        if (success)
        {
            return Tuple.Create<HttpStatusCode, ConferenceEditorViewModel?>(HttpStatusCode.OK, conference);
        }
        else
        {
            return Tuple.Create<HttpStatusCode, ConferenceEditorViewModel?>(HttpStatusCode.BadRequest, null);
        }
    }

    public async Task<Tuple<HttpStatusCode, string?>> DeleteConference(string conferenceId)
    {
        bool success = await conferenceService.DeleteConference(conferenceId);
        if (success)
        {
            return Tuple.Create(HttpStatusCode.OK, conferenceId);
        }
        else
        {
            return Tuple.Create<HttpStatusCode, string?>(HttpStatusCode.BadRequest, null);
        }
    }

    /// <summary>
    /// Get conference by name.
    /// </summary>
    /// <param name="conferenceName">Name of a conference.</param>
    /// <returns>Conference.</returns>
    private async Task<DAL.Model.Conference?> GetConferenceByName(string conferenceName) =>
        await conferenceService.GetConferenceByName(conferenceName);
}
