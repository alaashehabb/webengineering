using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.MedicalService;

public class CreateMedicalServiceDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [Range(1, 6)]
    public int CreditHours { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A valid DoctorId is required.")]
    public int DoctorId { get; set; }
}
