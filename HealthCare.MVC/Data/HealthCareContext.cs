using HealthCare.MVC.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.MVC.Data
{
    public partial class HealthCareContext : DbContext
    {
        public HealthCareContext()
        {
        }

        public HealthCareContext(DbContextOptions<HealthCareContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Agent> Agents { get; set; } = null!;
        public virtual DbSet<Asign> Asigns { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Note> Notes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local); database = HealthCare;uid=sa;pwd=12345;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>(entity =>
            {
                entity.ToTable("Agent");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Role)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("('User')");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Asign>(entity =>
            {
                entity.ToTable("Asign");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AgentId).HasColumnName("AgentID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.Asigns)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Asign__AgentID__35BCFE0A");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Asigns)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Asign__CustomerI__36B12243");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.BusinessType).HasMaxLength(100);

                entity.Property(e => e.CapitalSize).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.ClassifyCode).HasMaxLength(20);

                entity.Property(e => e.CompanyName).HasMaxLength(100);

                entity.Property(e => e.ContactGender).HasMaxLength(20);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.County).HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.HaveChildren).HasDefaultValueSql("((0))");

                entity.Property(e => e.HomeOwner).HasDefaultValueSql("((0))");

                entity.Property(e => e.HomePage).HasMaxLength(100);

                entity.Property(e => e.HomePhone)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IsMarried).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSelfEmployed).HasDefaultValueSql("((0))");

                entity.Property(e => e.JobTitle).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Latitude).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Nationality).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PostalCode).HasMaxLength(20);

                entity.Property(e => e.Region).HasMaxLength(100);

                entity.Property(e => e.State).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.ToTable("Note");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AgentId).HasColumnName("AgentID");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Note__CustomerID__3B75D760");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
