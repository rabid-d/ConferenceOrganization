namespace DAL.Model
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Degree { get; set; }
        public string Work { get; set; }
        public string Position { get; set; }
        public string ProfessionalBiography { get; set; }
        public string PathToPhoto { get; set; }

        public virtual ICollection<Section> Sections { get; set; }
        public virtual ICollection<Talk> Talks { get; set; }

        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
