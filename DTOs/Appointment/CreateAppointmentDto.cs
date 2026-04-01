using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.Appointment;

public class CreateAppointmentDto
{
    [Range(1, int.MaxValue, ErrorMessage = "A valid PatientId is required.")]
    public int PatientId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A valid MedicalServiceId is required.")]
    public int MedicalServiceId { get; set; }
}
