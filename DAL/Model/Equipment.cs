namespace DAL.Model
{
    public class Equipment
    {
        public Guid EquipmentId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Talk> Talks { get; set; }

        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
