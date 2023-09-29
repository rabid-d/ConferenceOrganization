namespace DAL.Model
{
    public class Talk
    {
        public Guid TalkId { get; set; }
        public string Theme { get; set; }
        public Guid SpeakerId { get; set; }
        public Guid SectionId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public virtual User Speaker { get; set; }
        public virtual Section Section { get; set; }
        public virtual ICollection<Equipment> Equipment { get; set; }

        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
