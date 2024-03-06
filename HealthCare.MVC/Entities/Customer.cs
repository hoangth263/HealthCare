namespace HealthCare.MVC.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Asigns = new HashSet<Asign>();
            Notes = new HashSet<Note>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Asign> Asigns { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
