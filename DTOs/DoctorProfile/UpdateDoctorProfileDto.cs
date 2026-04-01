using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.DoctorProfile;

public class UpdateDoctorProfileDto
{
    [MaxLength(500)]
    public string? Bio { get; set; }

    [MaxLength(200)]
    public string? OfficeLocation { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(200)]
    public string? LinkedInUrl { get; set; }
}
