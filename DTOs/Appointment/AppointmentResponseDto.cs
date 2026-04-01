using CourseManagementAPI.Models;

namespace CourseManagementAPI.DTOs.Appointment;

public class AppointmentResponseDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public int MedicalServiceId { get; set; }
    public string MedicalServiceTitle { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }
    public double? Grade { get; set; }
    public DateTime ScheduledAt { get; set; }
}
