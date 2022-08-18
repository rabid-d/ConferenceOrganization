namespace DAL.Model
{
    public class Section
    {
        public Guid SectionId { get; set; }
        public string Name { get; set; }
        public Guid ChairpersonId { get; set; }
        public string Room { get; set; }
        public Guid ConferenceId { get; set; }

        public virtual User Chairperson { get; set; }
        public virtual Conference Conference { get; set; }
        public virtual ICollection<Talk> Talks { get; set; }

        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
