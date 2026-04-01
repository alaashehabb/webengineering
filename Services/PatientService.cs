using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Patient;
using CourseManagementAPI.Interfaces;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public class PatientService : IPatientService
{
    private readonly ApplicationDbContext _db;

    public PatientService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PatientResponseDto>> GetAllAsync()
    {
        return await _db.Patients
            .AsNoTracking()
            .Select(s => new PatientResponseDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PatientNumber = s.PatientNumber,
                DateOfBirth = s.DateOfBirth,
                ScheduledAt = s.ScheduledAt
            })
            .ToListAsync();
    }

    public async Task<PatientResponseDto?> GetByIdAsync(int id)
    {
        return await _db.Patients
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new PatientResponseDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PatientNumber = s.PatientNumber,
                DateOfBirth = s.DateOfBirth,
                ScheduledAt = s.ScheduledAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PatientResponseDto> CreateAsync(CreatePatientDto dto)
    {
        var emailExists = await _db.Patients
            .AsNoTracking()
            .AnyAsync(s => s.Email == dto.Email);

        if (emailExists)
            throw new InvalidOperationException("Patient email already exists.");

        var numberExists = await _db.Patients
            .AsNoTracking()
            .AnyAsync(s => s.PatientNumber == dto.PatientNumber);

        if (numberExists)
            throw new InvalidOperationException("Patient number already exists.");

        var patient = new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PatientNumber = dto.PatientNumber,
            DateOfBirth = dto.DateOfBirth
        };

        _db.Patients.Add(patient);
        await _db.SaveChangesAsync();

        return new PatientResponseDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Email = patient.Email,
            PatientNumber = patient.PatientNumber,
            DateOfBirth = patient.DateOfBirth,
            ScheduledAt = patient.ScheduledAt
        };
    }

    public async Task<PatientResponseDto?> UpdateAsync(int id, UpdatePatientDto dto)
    {
        var patient = await _db.Patients.FindAsync(id);
        if (patient is null) return null;

        patient.FirstName = dto.FirstName;
        patient.LastName = dto.LastName;

        await _db.SaveChangesAsync();

        return new PatientResponseDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Email = patient.Email,
            PatientNumber = patient.PatientNumber,
            DateOfBirth = patient.DateOfBirth,
            ScheduledAt = patient.ScheduledAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _db.Patients.FindAsync(id);
        if (patient is null) return false;

        _db.Patients.Remove(patient);
        await _db.SaveChangesAsync();
        return true;
    }
}
