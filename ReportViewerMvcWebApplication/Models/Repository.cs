using DAL.Model;
using DAL.Services;

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
        Conference conference = await GetConference(conferenceName);
        IList<string> schedule = new List<string>();
        schedule.Add($"Schedule for conference: «{conferenceName}»");
        foreach (Section section in conference.Sections)
        {
            schedule.Add($"Section: {section.Name} Chairperson: {section.Chairperson.FullName}; Room: {section.Room};");
            foreach (Talk talk in section.Talks)
            {
                schedule.Add($" - Talk: {talk.Theme}; {talk.Speaker.FullName}; {talk.DateStart.ToShortTimeString()} - {talk.DateEnd.ToShortTimeString()};");
            }
        }
        return schedule;
    }

    public async Task<IList<string>> GetConfParticipants(string conferenceName)
    {
        Conference conference = await GetConference(conferenceName);
        IList<string> participants = new List<string>();
        participants.Add("Chairpersons:");
        foreach (Section section in conference.Sections)
        {
            participants.Add($" - {section.Chairperson.FullName}; {section.Chairperson.Work}; {section.Chairperson.Position};");
        }
        participants.Add("Speakers:");
        foreach (Section section in conference.Sections)
        {
            foreach (Talk talk in section.Talks)
            {
                participants.Add($" - {talk.Speaker.FullName}; {talk.Speaker.Work}; {talk.Speaker.Position};");
            }
        }
        return participants;
    }

    public async Task<IList<string>> GetConfEquipment(string conferenceName)
    {
        Conference conference = await GetConference(conferenceName);
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
        equipmentOutput.Add($"Needed equipment for the conference «{conferenceName}»:");
        foreach (Equipment equip in equipment)
        {
            equipmentOutput.Add(equip.Name);
            foreach (Section section in conference.Sections)
            {
                foreach (Talk talk in section.Talks)
                {
                    if (talk.Equipment.Contains(equip))
                    {
                        equipmentOutput.Add($" - Room: {section.Room}; Time: {talk.DateStart.ToShortTimeString()} - {talk.DateEnd.ToShortTimeString()};");
                    }
                }
            }
        }
        return equipmentOutput;
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

    private async Task<string> UploadPhoto(string userName, string userId, IFormFile photo)
    {
        string filePath = await GetNewUserPathToPhoto(userName, userId, photo.FileName);
        Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photos"));
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
        string[] paths = { AppDomain.CurrentDomain.BaseDirectory, "photos", newPhotoName };
        string newPath = Path.Combine(paths);
        newPath = Path.ChangeExtension(newPath, extension);
        return newPath;
    }

    private async Task<Conference> GetConference(string conferenceName)
    {
        Conference? conference = await conferenceService.GetConferencesByName(conferenceName);
        return conference ?? throw new InvalidOperationException();
    }
}
