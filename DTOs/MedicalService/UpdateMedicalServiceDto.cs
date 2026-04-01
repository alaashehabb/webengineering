using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.MedicalService;

public class UpdateMedicalServiceDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [Range(1, 6)]
    public int CreditHours { get; set; }

    public bool IsActive { get; set; }
}
