using CourseManagementAPI.DTOs.MedicalService;

namespace CourseManagementAPI.Interfaces;

public interface IMedicalServiceService
{
    Task<IEnumerable<MedicalServiceResponseDto>> GetAllAsync();
    Task<MedicalServiceResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<MedicalServiceResponseDto>> GetByDoctorAsync(int doctorId);
    Task<MedicalServiceResponseDto> CreateAsync(CreateMedicalServiceDto dto);
    Task<MedicalServiceResponseDto?> UpdateAsync(int id, UpdateMedicalServiceDto dto);
    Task<bool> DeleteAsync(int id);
}
