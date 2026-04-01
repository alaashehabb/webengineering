using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.MedicalService;
using CourseManagementAPI.Interfaces;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public class MedicalServiceService : IMedicalServiceService
{
    private readonly ApplicationDbContext _db;

    public MedicalServiceService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<MedicalServiceResponseDto>> GetAllAsync()
    {
        return await _db.MedicalServices
            .AsNoTracking()
            .Select(c => new MedicalServiceResponseDto
            {
                Id = c.Id,
                Title = c.Title,
                Code = c.Code,
                Description = c.Description,
                CreditHours = c.CreditHours,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                DoctorId = c.DoctorId,
                DoctorFullName = c.Doctor.FirstName + " " + c.Doctor.LastName
            })
            .ToListAsync();
    }

    public async Task<MedicalServiceResponseDto?> GetByIdAsync(int id)
    {
        return await _db.MedicalServices
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new MedicalServiceResponseDto
            {
                Id = c.Id,
                Title = c.Title,
                Code = c.Code,
                Description = c.Description,
                CreditHours = c.CreditHours,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                DoctorId = c.DoctorId,
                DoctorFullName = c.Doctor.FirstName + " " + c.Doctor.LastName
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<MedicalServiceResponseDto>> GetByDoctorAsync(int doctorId)
    {
        return await _db.MedicalServices
            .AsNoTracking()
            .Where(c => c.DoctorId == doctorId)
            .Select(c => new MedicalServiceResponseDto
            {
                Id = c.Id,
                Title = c.Title,
                Code = c.Code,
                Description = c.Description,
                CreditHours = c.CreditHours,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                DoctorId = c.DoctorId,
                DoctorFullName = c.Doctor.FirstName + " " + c.Doctor.LastName
            })
            .ToListAsync();
    }

    public async Task<MedicalServiceResponseDto> CreateAsync(CreateMedicalServiceDto dto)
    {
        var doctorExists = await _db.Doctors
            .AsNoTracking()
            .AnyAsync(i => i.Id == dto.DoctorId);

        if (!doctorExists)
            throw new KeyNotFoundException("Doctor not found.");

        var codeExists = await _db.MedicalServices
            .AsNoTracking()
            .AnyAsync(c => c.Code == dto.Code);

        if (codeExists)
            throw new InvalidOperationException("MedicalService code already exists.");

        var medicalService = new MedicalService
        {
            Title = dto.Title,
            Code = dto.Code,
            Description = dto.Description,
            CreditHours = dto.CreditHours,
            DoctorId = dto.DoctorId
        };

        _db.MedicalServices.Add(medicalService);
        await _db.SaveChangesAsync();

        await _db.Entry(medicalService).Reference(c => c.Doctor).LoadAsync();

        return new MedicalServiceResponseDto
        {
            Id = medicalService.Id,
            Title = medicalService.Title,
            Code = medicalService.Code,
            Description = medicalService.Description,
            CreditHours = medicalService.CreditHours,
            IsActive = medicalService.IsActive,
            CreatedAt = medicalService.CreatedAt,
            DoctorId = medicalService.DoctorId,
            DoctorFullName = medicalService.Doctor.FirstName + " " + medicalService.Doctor.LastName
        };
    }

    public async Task<MedicalServiceResponseDto?> UpdateAsync(int id, UpdateMedicalServiceDto dto)
    {
        var medicalService = await _db.MedicalServices
            .Include(c => c.Doctor)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (medicalService is null) return null;

        medicalService.Title = dto.Title;
        medicalService.Description = dto.Description;
        medicalService.CreditHours = dto.CreditHours;
        medicalService.IsActive = dto.IsActive;

        await _db.SaveChangesAsync();

        return new MedicalServiceResponseDto
        {
            Id = medicalService.Id,
            Title = medicalService.Title,
            Code = medicalService.Code,
            Description = medicalService.Description,
            CreditHours = medicalService.CreditHours,
            IsActive = medicalService.IsActive,
            CreatedAt = medicalService.CreatedAt,
            DoctorId = medicalService.DoctorId,
            DoctorFullName = medicalService.Doctor.FirstName + " " + medicalService.Doctor.LastName
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var medicalService = await _db.MedicalServices.FindAsync(id);
        if (medicalService is null) return false;

        _db.MedicalServices.Remove(medicalService);
        await _db.SaveChangesAsync();
        return true;
    }
}
