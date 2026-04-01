using System.ComponentModel.DataAnnotations;
using CourseManagementAPI.Models;

namespace CourseManagementAPI.DTOs.Appointment;

public class UpdateAppointmentDto
{
    [Required]
    public AppointmentStatus Status { get; set; }

    [Range(0.0, 4.0)]
    public double? Grade { get; set; }
}
