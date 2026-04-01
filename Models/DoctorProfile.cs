using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.Models;

public class DoctorProfile
{
    public int Id { get; set; }

    [MaxLength(500)]
    public string? Bio { get; set; }

    [MaxLength(200)]
    public string? OfficeLocation { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(200)]
    public string? LinkedInUrl { get; set; }

    // One-to-one FK back to Doctor
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
}
