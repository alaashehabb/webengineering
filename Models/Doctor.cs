using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.Models;

public class Doctor
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
    [MaxLength(100)]
    public string Department { get; set; } = string.Empty;

    public DateTime HiredAt { get; set; } = DateTime.UtcNow;

    // One-to-one: each doctor has one profile
    public DoctorProfile? Profile { get; set; }

    // One-to-many: an doctor can teach many medicalServices
    public ICollection<MedicalService> MedicalServices { get; set; } = new List<MedicalService>();
}
