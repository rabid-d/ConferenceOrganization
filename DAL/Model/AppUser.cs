namespace DAL.Model;

public class AppUser
{
    public Guid AppUserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}
