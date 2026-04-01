using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<DoctorProfile> DoctorProfiles => Set<DoctorProfile>();
    public DbSet<MedicalService> MedicalServices => Set<MedicalService>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── One-to-One: Doctor ↔ DoctorProfile ──────────────────────
        modelBuilder.Entity<DoctorProfile>()
            .HasOne(p => p.Doctor)
            .WithOne(i => i.Profile)
            .HasForeignKey<DoctorProfile>(p => p.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── One-to-Many: Doctor → MedicalServices ───────────────────────────────
        modelBuilder.Entity<MedicalService>()
            .HasOne(c => c.Doctor)
            .WithMany(i => i.MedicalServices)
            .HasForeignKey(c => c.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── Many-to-Many: Patient ↔ MedicalService via Appointment ───────────────────
        modelBuilder.Entity<Appointment>()
            .HasOne(e => e.Patient)
            .WithMany(s => s.Appointments)
            .HasForeignKey(e => e.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasOne(e => e.MedicalService)
            .WithMany(c => c.Appointments)
            .HasForeignKey(e => e.MedicalServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Prevent duplicate appointments for the same patient/medicalService pair
        modelBuilder.Entity<Appointment>()
            .HasIndex(e => new { e.PatientId, e.MedicalServiceId })
            .IsUnique();

        // ── Column constraints ───────────────────────────────────────────────
        modelBuilder.Entity<Patient>()
            .HasIndex(s => s.Email)
            .IsUnique();

        modelBuilder.Entity<Patient>()
            .HasIndex(s => s.PatientNumber)
            .IsUnique();

        modelBuilder.Entity<Doctor>()
            .HasIndex(i => i.Email)
            .IsUnique();

        modelBuilder.Entity<MedicalService>()
            .HasIndex(c => c.Code)
            .IsUnique();

        modelBuilder.Entity<Appointment>()
            .Property(e => e.Status)
            .HasConversion<string>();

        // ── AppUser ──────────────────────────────────────────────────────────
        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .Property(u => u.Role)
            .HasConversion<string>();
    }
}
