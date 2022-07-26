using DAL.Model;

namespace DAL.Services
{
    public class UserService
    {
        private ConferenceDatabaseContext confDbContext = new();

        public async Task AddUser(Guid id, string fullName, string degree, string work, string position, string biography)
        {
            User newUser = new()
            {
                UserId = id,
                FullName = fullName,
                Degree = degree,
                Work = work,
                Position = position,
                ProfessionalBiography = biography,
            };
            await confDbContext.Users.AddAsync(newUser);
            await confDbContext.SaveChangesAsync();
        }

        public async Task<bool> IsUserExists(string id)
        {
            return confDbContext.Users.Any(u => u.UserId.ToString().ToLower() == id.ToLower());
        }

        public async Task UpdateUserPhoto(string id, string pathToPhoto)
        {
            User? user = confDbContext.Users.FirstOrDefault(u => u.UserId.ToString().ToLower() == id.ToLower());
            if (user != null)
            {
                user.PathToPhoto = pathToPhoto;
                await confDbContext.SaveChangesAsync();
            }
        }
    }
}
