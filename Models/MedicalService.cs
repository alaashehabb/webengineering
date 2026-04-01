using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.Models;

public class MedicalService
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(1, 6)]
    public int CreditHours { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Many-to-one: a medicalService belongs to one doctor
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    // Many-to-many: a medicalService can have many scheduled patients
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
