using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Doctor;
using CourseManagementAPI.DTOs.DoctorProfile;
using CourseManagementAPI.Interfaces;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public class DoctorService : IDoctorService
{
    private readonly ApplicationDbContext _db;

    public DoctorService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<DoctorResponseDto>> GetAllAsync()
    {
        return await _db.Doctors
            .AsNoTracking()
            .Select(i => new DoctorResponseDto
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Email = i.Email,
                Department = i.Department,
                HiredAt = i.HiredAt
            })
            .ToListAsync();
    }

    public async Task<DoctorResponseDto?> GetByIdAsync(int id)
    {
        return await _db.Doctors
            .AsNoTracking()
            .Where(i => i.Id == id)
            .Select(i => new DoctorResponseDto
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Email = i.Email,
                Department = i.Department,
                HiredAt = i.HiredAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DoctorResponseDto> CreateAsync(CreateDoctorDto dto)
    {
        var emailExists = await _db.Doctors
            .AsNoTracking()
            .AnyAsync(i => i.Email == dto.Email);

        if (emailExists)
            throw new InvalidOperationException("Doctor email already exists.");

        var doctor = new Doctor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Department = dto.Department
        };

        _db.Doctors.Add(doctor);
        await _db.SaveChangesAsync();

        return new DoctorResponseDto
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Email = doctor.Email,
            Department = doctor.Department,
            HiredAt = doctor.HiredAt
        };
    }

    public async Task<DoctorResponseDto?> UpdateAsync(int id, UpdateDoctorDto dto)
    {
        var doctor = await _db.Doctors.FindAsync(id);
        if (doctor is null) return null;

        doctor.FirstName = dto.FirstName;
        doctor.LastName = dto.LastName;
        doctor.Department = dto.Department;

        await _db.SaveChangesAsync();

        return new DoctorResponseDto
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Email = doctor.Email,
            Department = doctor.Department,
            HiredAt = doctor.HiredAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var doctor = await _db.Doctors.FindAsync(id);
        if (doctor is null) return false;

        _db.Doctors.Remove(doctor);
        await _db.SaveChangesAsync();
        return true;
    }

    // ── DoctorProfile ────────────────────────────────────────────────────

    public async Task<DoctorProfileResponseDto?> GetProfileAsync(int doctorId)
    {
        return await _db.DoctorProfiles
            .AsNoTracking()
            .Where(p => p.DoctorId == doctorId)
            .Select(p => new DoctorProfileResponseDto
            {
                Id = p.Id,
                DoctorId = p.DoctorId,
                Bio = p.Bio,
                OfficeLocation = p.OfficeLocation,
                PhoneNumber = p.PhoneNumber,
                LinkedInUrl = p.LinkedInUrl
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DoctorProfileResponseDto> CreateProfileAsync(CreateDoctorProfileDto dto)
    {
        var doctorExists = await _db.Doctors
            .AsNoTracking()
            .AnyAsync(i => i.Id == dto.DoctorId);

        if (!doctorExists)
            throw new KeyNotFoundException("Doctor not found.");

        var profileExists = await _db.DoctorProfiles
            .AsNoTracking()
            .AnyAsync(p => p.DoctorId == dto.DoctorId);

        if (profileExists)
            throw new InvalidOperationException("Profile already exists for this doctor.");

        var profile = new DoctorProfile
        {
            DoctorId = dto.DoctorId,
            Bio = dto.Bio,
            OfficeLocation = dto.OfficeLocation,
            PhoneNumber = dto.PhoneNumber,
            LinkedInUrl = dto.LinkedInUrl
        };

        _db.DoctorProfiles.Add(profile);
        await _db.SaveChangesAsync();

        return new DoctorProfileResponseDto
        {
            Id = profile.Id,
            DoctorId = profile.DoctorId,
            Bio = profile.Bio,
            OfficeLocation = profile.OfficeLocation,
            PhoneNumber = profile.PhoneNumber,
            LinkedInUrl = profile.LinkedInUrl
        };
    }

    public async Task<DoctorProfileResponseDto?> UpdateProfileAsync(int doctorId, UpdateDoctorProfileDto dto)
    {
        var doctorExists = await _db.Doctors
            .AsNoTracking()
            .AnyAsync(i => i.Id == doctorId);

        if (!doctorExists) return null;

        var profile = await _db.DoctorProfiles
            .FirstOrDefaultAsync(p => p.DoctorId == doctorId);

        if (profile is null)
        {
            profile = new DoctorProfile
            {
                DoctorId = doctorId,
                Bio = dto.Bio,
                OfficeLocation = dto.OfficeLocation,
                PhoneNumber = dto.PhoneNumber,
                LinkedInUrl = dto.LinkedInUrl
            };

            _db.DoctorProfiles.Add(profile);
            await _db.SaveChangesAsync();

            return new DoctorProfileResponseDto
            {
                Id = profile.Id,
                DoctorId = profile.DoctorId,
                Bio = profile.Bio,
                OfficeLocation = profile.OfficeLocation,
                PhoneNumber = profile.PhoneNumber,
                LinkedInUrl = profile.LinkedInUrl
            };
        }

        profile.Bio = dto.Bio;
        profile.OfficeLocation = dto.OfficeLocation;
        profile.PhoneNumber = dto.PhoneNumber;
        profile.LinkedInUrl = dto.LinkedInUrl;

        await _db.SaveChangesAsync();

        return new DoctorProfileResponseDto
        {
            Id = profile.Id,
            DoctorId = profile.DoctorId,
            Bio = profile.Bio,
            OfficeLocation = profile.OfficeLocation,
            PhoneNumber = profile.PhoneNumber,
            LinkedInUrl = profile.LinkedInUrl
        };
    }
}
