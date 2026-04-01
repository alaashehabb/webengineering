using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.Patient;

public class UpdatePatientDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
}
