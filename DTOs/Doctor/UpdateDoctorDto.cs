using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.Doctor;

public class UpdateDoctorDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Department { get; set; } = string.Empty;
}
