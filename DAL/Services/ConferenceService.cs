using DAL.Model;

namespace DAL.Services
{
    public class ConferenceService
    {
        private ConferenceDatabaseContext confDbContext = new();

        public async Task<List<Conference>> GetAllConferences()
        {
            return confDbContext.Conferences.ToList();
        }

        public async Task<Conference?> GetConferenceByNumber(string conferenceNumber)
        {
            return confDbContext.Conferences.Where(c => c.Name.StartsWith($"Conference {conferenceNumber}")).FirstOrDefault();
        }

        public async Task<List<string>> GetConferencesNames()
        {
            List<string> conferencesNames = new();
            foreach (Conference conf in confDbContext.Conferences)
            {
                conferencesNames.Add(conf.Name);
            }
            return conferencesNames;
        }

        public async Task<Conference?> GetConferencesByName(string name)
        {
            return confDbContext.Conferences.Where(c => c.Name == name).FirstOrDefault();
        }

        public async Task<Conference?> GetConferenceById(string id)
        {
            Guid confId = new(id);
            return confDbContext.Conferences.Where(c => c.ConferenceId == confId).FirstOrDefault();
        }

        public async Task UpdateConference(Conference conf, string id)
        {
            Conference? conference = await GetConferenceById(id);
            if (conference == null)
            {
                return;
            }
            conference.Name = conf.Name;
            conference.Address = conf.Address;
            conference.DateStart = conf.DateStart;
            conference.DateEnd = conf.DateEnd;
            await confDbContext.SaveChangesAsync();
        }

        public async Task DeleteConference(string id)
        {
            Conference? conference = await GetConferenceById(id);
            if (conference == null)
            {
                return;
            }
            confDbContext.Conferences.Remove(conference);
            await confDbContext.SaveChangesAsync();
        }

        public async Task AddNewConference(Conference conference)
        {
            confDbContext.Conferences.Add(conference);
            await confDbContext.SaveChangesAsync();
        }
    }
}
