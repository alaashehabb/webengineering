namespace CourseManagementAPI.DTOs.Patient;

public class PatientResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PatientNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime ScheduledAt { get; set; }
}
