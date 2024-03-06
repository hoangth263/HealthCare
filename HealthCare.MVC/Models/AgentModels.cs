using HealthCare.MVC.Entities;

namespace HealthCare.MVC.Models
{
    public class AgentViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Role { get; set; } = null!;
        public virtual ICollection<Asign> Asigns { get; set; }
    }

    public class AgentDetailsModel : AgentViewModel
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class AgentCreateModel
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }

    public class AgentUpdateModel : AgentCreateModel
    {
        public int Id { get; set; }
    }
}
