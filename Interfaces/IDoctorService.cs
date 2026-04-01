using CourseManagementAPI.DTOs.Doctor;
using CourseManagementAPI.DTOs.DoctorProfile;

namespace CourseManagementAPI.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorResponseDto>> GetAllAsync();
    Task<DoctorResponseDto?> GetByIdAsync(int id);
    Task<DoctorResponseDto> CreateAsync(CreateDoctorDto dto);
    Task<DoctorResponseDto?> UpdateAsync(int id, UpdateDoctorDto dto);
    Task<bool> DeleteAsync(int id);

    Task<DoctorProfileResponseDto?> GetProfileAsync(int doctorId);
    Task<DoctorProfileResponseDto> CreateProfileAsync(CreateDoctorProfileDto dto);
    Task<DoctorProfileResponseDto?> UpdateProfileAsync(int doctorId, UpdateDoctorProfileDto dto);
}
