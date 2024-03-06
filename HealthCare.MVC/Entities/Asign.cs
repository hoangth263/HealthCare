namespace HealthCare.MVC.Entities
{
    public partial class Asign
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Agent Agent { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
    }
}
