using HealthCare.MVC.Entities;

namespace HealthCare.MVC.Models
{
    public class NoteCreateModel
    {
        public int CustomerId { get; set; }
        public int AgentId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
    }

    public class NoteUpdateModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AgentId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class NoteViewModel : NoteUpdateModel
    {
        public virtual Agent Agent { get; set; } = null!;

    }

    public enum NoteType
    {
        F, O, R, M, X
    }

}
