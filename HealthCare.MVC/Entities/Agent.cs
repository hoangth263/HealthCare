namespace HealthCare.MVC.Entities
{
    public partial class Agent
    {
        public Agent()
        {
            Asigns = new HashSet<Asign>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Asign> Asigns { get; set; }
    }
}
