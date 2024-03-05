using HealthCare.MVC.Entities;

namespace HealthCare.MVC.Models
{
    public class AsignCreateModel
    {
        public int AgentId { get; set; }
        public int CustomerId { get; set; }
    }


    public class AsignUpdateModel
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int CustomerId { get; set; }
    }

    public class AsignDetailsModel
    {
        public int Id { get; set; }
        public AgentViewModel Agent { get; set; }
        public Customer Customer { get; set; }
        public IList<Note> Notes { get; set; }
    }
}
