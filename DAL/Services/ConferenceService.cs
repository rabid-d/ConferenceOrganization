using DAL.Model;

namespace DAL.Services
{
    public class ConferenceService
    {
        private ConferenceDatabaseContext confDbContext = new ConferenceDatabaseContext();

        public List<Conference> GetAllConferences()
        {
            return confDbContext.Conferences.ToList();
        }

        public Conference? GetConferenceByNumber(string conferenceNumber)
        {
            return confDbContext.Conferences.Where(c => c.Name.StartsWith($"Conference {conferenceNumber}")).FirstOrDefault();
        }
    }
}
