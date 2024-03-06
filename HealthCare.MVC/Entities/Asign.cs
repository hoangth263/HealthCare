namespace HealthCare.MVC.Entities
{
    public partial class Asign
    {
        public Asign()
        {
            Notes = new HashSet<Note>();
        }

        public int Id { get; set; }
        public int AgentId { get; set; }
        public int CustomerId { get; set; }

        public virtual Agent Agent { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<Note> Notes { get; set; }
    }
}
