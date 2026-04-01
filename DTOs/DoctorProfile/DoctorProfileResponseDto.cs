namespace CourseManagementAPI.DTOs.DoctorProfile;

public class DoctorProfileResponseDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string? Bio { get; set; }
    public string? OfficeLocation { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LinkedInUrl { get; set; }
}
