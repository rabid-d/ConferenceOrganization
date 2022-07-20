using DAL.Model;

namespace DAL.Services
{
    public class UserService
    {
        private ConferenceDatabaseContext confDbContext = new ConferenceDatabaseContext();

        public void AddUser(Guid id, string fullName, string degree, string work, string position, string biography, string pathToPhoto)
        {
            var newUser = new User() 
            { 
                UserId = id,
                FullName = fullName,
                Degree = degree,
                Work = work,
                Position = position,
                ProfessionalBiography = biography,
                PathToPhoto = pathToPhoto,
            };
            confDbContext.Users.Add(newUser);
            confDbContext.SaveChanges();
        }

        public bool IsUserExists(string id)
        {
            return confDbContext.Users.Any(u => u.UserId.ToString().ToLower() == id.ToLower());
        }
    }
}
