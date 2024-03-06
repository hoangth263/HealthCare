namespace HealthCare.MVC.Models
{
    public class NoteCreateModel
    {
        public int AsignId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
    }

    public class NoteUpdateModel
    {
        public int Id { get; set; }
        public int AsignId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public enum NoteType
    {
        F, O, R, M, X
    }

}
