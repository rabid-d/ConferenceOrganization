using DAL.Model;
using DAL.Services;
using ReportViewerMvcWebApplication.Resources;

namespace ReportViewerMvcWebApplication.Models;

public class Repository
{
    private readonly ConferenceService conferenceService;
    private readonly UserService userService;

    public Repository(ConferenceService conferenceService, UserService userService)
    {
        this.conferenceService = conferenceService;
        this.userService = userService;
    }

    public async Task<IList<string>> GetConferencesNames()
    {
        return await conferenceService.GetConferencesNames();
    }

    public async Task<IList<string>> GetConfSchedule(string conferenceName)
    {
        DAL.Model.Conference conference = await GetConference(conferenceName);
        IList<string> schedule = new List<string>();
        schedule.Add(string.Format(Resource.ScheduleForConference, conferenceName));
        foreach (Section section in conference.Sections)
        {
            schedule.Add(string.Format(Resource.SectionEntry, section.Name, section.Chairperson.FullName, section.Room));
            foreach (Talk talk in section.Talks)
            {
                schedule.Add(string.Format(Resource.TalkEntry, talk.Theme, talk.Speaker.FullName, talk.DateStart.ToShortTimeString(), talk.DateEnd.ToShortTimeString()));
            }
        }
        return schedule;
    }

    public async Task<IList<string>> GetConfParticipants(string conferenceName)
    {
        DAL.Model.Conference conference = await GetConference(conferenceName);
        IList<string> participants = new List<string>();
        participants.Add(Resource.Chairpersons);
        foreach (Section section in conference.Sections)
        {
            participants.Add(string.Format(Resource.ChairpersonEntry, section.Chairperson.FullName, section.Chairperson.Work, section.Chairperson.Position));
        }
        participants.Add(Resource.Speakers);
        foreach (Section section in conference.Sections)
        {
            foreach (Talk talk in section.Talks)
            {
                participants.Add(string.Format(Resource.SpeakerEntry, talk.Speaker.FullName, talk.Speaker.Work, talk.Speaker.Position));
            }
        }
        return participants;
    }

    public async Task<IList<string>> GetConfEquipment(string conferenceName)
    {
        DAL.Model.Conference conference = await GetConference(conferenceName);
        HashSet<Equipment> equipment = new();
        IList<string> equipmentOutput = new List<string>();
        foreach (Section section in conference.Sections)
        {
            foreach (Talk talk in section.Talks)
            {
                foreach (Equipment equip in talk.Equipment)
                {
                    equipment.Add(equip);
                }
            }
        }
        equipmentOutput.Add(string.Format(Resource.NeededEquipment, conferenceName));
        foreach (Equipment equip in equipment)
        {
            equipmentOutput.Add(equip.Name);
            foreach (Section section in conference.Sections)
            {
                foreach (Talk talk in section.Talks)
                {
                    if (talk.Equipment.Contains(equip))
                    {
                        equipmentOutput.Add(string.Format(Resource.RoomEntry, section.Room, talk.DateStart.ToShortTimeString(), talk.DateEnd.ToShortTimeString()));
                    }
                }
            }
        }
        return equipmentOutput;
    }

    public async Task<DAL.Model.Conference?> GetConferenceById(string id)
    {
        return await conferenceService.GetConferenceById(id);
    }

    public async Task UpateConferende(Conference conf, string id)
    {
        DAL.Model.Conference conference = new() { Name = conf.Name, Address = conf.Address, DateStart = conf.DateStart, DateEnd = conf.DateEnd };
        await conferenceService.UpdateConference(conference, id);
    }

    public async Task DeleteConference(string id)
    {
        await conferenceService.DeleteConference(id);
    }

    public async Task AddUser(string name, string degree, string work, string positon, string bio, IFormFile photo)
    {
        Guid userId = Guid.NewGuid();
        await userService.AddUser(
            userId,
            name,
            degree,
            work,
            positon,
            bio
        );
        if (photo != null)
        {
            string userPhotoPath = await UploadPhoto(name, userId.ToString(), photo);
            await userService.UpdateUserPhoto(userId.ToString(), userPhotoPath);
        }
    }

    public async Task AddAppUser(AppUser appUser)
    {
        DAL.Model.AppUser newAppUser = new() { Email = appUser.Email, Password = appUser.Password };
        await userService.AddAppUser(newAppUser);
    }

    public async Task<bool> IsLoginCredentialsValid(AppUser appUser)
    {
        DAL.Model.AppUser dalAppUser = new() { Email = appUser.Email, Password = appUser.Password };
        return await userService.IsLoginCredentialsValid(dalAppUser);
    }

    public async Task<IList<DAL.Model.Conference>> GetAllConferences()
    {
        return await conferenceService.GetAllConferences();
    }

    public async Task AddConference(Conference conf)
    {
        Guid newConfId = Guid.NewGuid();
        DAL.Model.Conference newConf = new()
        {
            ConferenceId = newConfId,
            Name = conf.Name,
            Address = conf.Address,
            DateStart = conf.DateStart,
            DateEnd = conf.DateEnd,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
        };
        await conferenceService.AddNewConference(newConf);
    }

    private async Task<string> UploadPhoto(string userName, string userId, IFormFile photo)
    {
        string filePath = await GetNewUserPathToPhoto(userName, userId, photo.FileName);
        Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Resource.Photos));
        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await photo.CopyToAsync(fileStream);
        }
        return filePath;
    }

    private async Task<string> GetNewUserPathToPhoto(string fullName, string id, string fileName)
    {
        string extension = Path.GetExtension(fileName);
        string newPhotoName = fullName.Trim().ToLower().Replace(' ', '_') + "_" + id;
        string[] paths = { AppDomain.CurrentDomain.BaseDirectory, Resource.Photos, newPhotoName };
        string newPath = Path.Combine(paths);
        newPath = Path.ChangeExtension(newPath, extension);
        return newPath;
    }

    private async Task<DAL.Model.Conference> GetConference(string conferenceName)
    {
        DAL.Model.Conference? conference = await conferenceService.GetConferencesByName(conferenceName);
        return conference ?? throw new InvalidOperationException();
    }
}
