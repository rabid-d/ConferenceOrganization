using DAL.Model;

namespace DAL.Services
{
    public class UserService
    {
        private ConferenceDatabaseContext confDbContext = new ConferenceDatabaseContext();

        public void AddUser(Guid id, string fullName, string degree, string work, string position, string biography)
        {
            var newUser = new User()
            {
                UserId = id,
                FullName = fullName,
                Degree = degree,
                Work = work,
                Position = position,
                ProfessionalBiography = biography,
            };
            confDbContext.Users.Add(newUser);
            confDbContext.SaveChanges();
        }

        public bool IsUserExists(string id)
        {
            return confDbContext.Users.Any(u => u.UserId.ToString().ToLower() == id.ToLower());
        }

        public void UpdateUserPhoto(string id, string pathToPhoto)
        {
            var user = confDbContext.Users.FirstOrDefault(u => u.UserId.ToString().ToLower() == id.ToLower());
            if (user != null)
            {
                user.PathToPhoto = pathToPhoto;
                confDbContext.SaveChanges();
            }
        }
    }
}
