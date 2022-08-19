using ReportViewerWebApi.ViewModels.ConferenceEquipment;
using ReportViewerWebApi.ViewModels.ConferencePersons;
using ReportViewerWebApi.ViewModels.ConferenceSchedule;

namespace ReportViewerWebApi.Services;

/// <summary>
/// Provides access to information about conferences.
/// </summary>
public class СonferenceService
{
    private readonly DAL.Services.ConferenceService conferenceService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="conferenceService">DAL service to work with conferences.</param>
    public СonferenceService(DAL.Services.ConferenceService conferenceService)
    {
        this.conferenceService = conferenceService;
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

    /// <summary>
    /// Get conference by name.
    /// </summary>
    /// <param name="conferenceName">Name of a conference.</param>
    /// <returns>Conference.</returns>
    private async Task<DAL.Model.Conference?> GetConferenceByName(string conferenceName) =>
        await conferenceService.GetConferencesByName(conferenceName);
}
