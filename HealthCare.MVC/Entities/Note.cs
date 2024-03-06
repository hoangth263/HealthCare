namespace HealthCare.MVC.Entities
{
    public partial class Note
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AgentId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Customer Customer { get; set; } = null!;
    }
}
