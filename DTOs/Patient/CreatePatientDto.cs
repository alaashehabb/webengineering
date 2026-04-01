using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.Patient;

public class CreatePatientDto
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
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(3)]
    [MaxLength(20)]
    public string PatientNumber { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }
}
