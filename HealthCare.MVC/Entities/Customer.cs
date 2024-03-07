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
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? CompanyName { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? State { get; set; }
        public string? County { get; set; }
        public string? HomePhone { get; set; }
        public string? HomePage { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string? JobTitle { get; set; }
        public string? ContactGender { get; set; }
        public int? EmployeeSize { get; set; }
        public decimal? CapitalSize { get; set; }
        public string? ClassifyCode { get; set; }
        public string? BusinessType { get; set; }
        public string? Nationality { get; set; }
        public bool? IsMarried { get; set; }
        public bool? HaveChildren { get; set; }
        public bool? HomeOwner { get; set; }
        public int? YearInBusiness { get; set; }
        public bool? IsSelfEmployed { get; set; }
        public string? DynamicInfo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Asign> Asigns { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
