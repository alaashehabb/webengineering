using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.Models;

public class Patient
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string PatientNumber { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public DateTime ScheduledAt { get; set; } = DateTime.UtcNow;

    // Many-to-many: a patient can enroll in many medicalServices
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
