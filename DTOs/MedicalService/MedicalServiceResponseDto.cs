namespace CourseManagementAPI.DTOs.MedicalService;

public class MedicalServiceResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CreditHours { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int DoctorId { get; set; }
    public string DoctorFullName { get; set; } = string.Empty;
}
