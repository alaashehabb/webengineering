using CourseManagementAPI.DTOs.Appointment;

namespace CourseManagementAPI.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();
    Task<IEnumerable<AppointmentResponseDto>> GetByPatientAsync(int patientId);
    Task<IEnumerable<AppointmentResponseDto>> GetByMedicalServiceAsync(int medicalServiceId);
    Task<AppointmentResponseDto?> GetByIdAsync(int id);
    Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto);
    Task<AppointmentResponseDto?> UpdateAsync(int id, UpdateAppointmentDto dto);
    Task<bool> DeleteAsync(int id);
}
