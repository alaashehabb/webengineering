using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.Models;

public enum AppointmentStatus
{
    Active,
    Completed,
    Dropped,
    Pending
}

public class Appointment
{
    public int Id { get; set; }

    public DateTime ScheduledAt { get; set; } = DateTime.UtcNow;

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Active;

    [Range(0.0, 4.0)]
    public double? Grade { get; set; }

    // Many-to-many join: FK to Patient
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    // Many-to-many join: FK to MedicalService
    public int MedicalServiceId { get; set; }
    public MedicalService MedicalService { get; set; } = null!;
}
