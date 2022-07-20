namespace DAL.Model
{
    public class Conference
    {
        public Guid ConferenceId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public virtual ICollection<Section> Sections { get; set; }

        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
