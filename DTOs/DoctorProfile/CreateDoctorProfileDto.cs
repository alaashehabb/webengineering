using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.DoctorProfile;

public class CreateDoctorProfileDto
{
    [Range(1, int.MaxValue, ErrorMessage = "A valid DoctorId is required.")]
    public int DoctorId { get; set; }

    [MinLength(10)]
    [MaxLength(500)]
    public string? Bio { get; set; }

    [MaxLength(200)]
    public string? OfficeLocation { get; set; }

    [MinLength(7)]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(200)]
    public string? LinkedInUrl { get; set; }
}
