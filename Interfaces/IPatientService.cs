using CourseManagementAPI.DTOs.Patient;

namespace CourseManagementAPI.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientResponseDto>> GetAllAsync();
    Task<PatientResponseDto?> GetByIdAsync(int id);
    Task<PatientResponseDto> CreateAsync(CreatePatientDto dto);
    Task<PatientResponseDto?> UpdateAsync(int id, UpdatePatientDto dto);
    Task<bool> DeleteAsync(int id);
}
