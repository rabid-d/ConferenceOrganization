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
    }
}
