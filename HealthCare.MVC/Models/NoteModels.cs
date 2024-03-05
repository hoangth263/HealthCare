namespace HealthCare.MVC.Models
{
    public class NoteCreateModel
    {
        public int AsignId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
    }

    public enum NoteType
    {
        F, O, R, M, X
    }

}
