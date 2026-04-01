using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Auth;
using CourseManagementAPI.Interfaces;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CourseManagementAPI.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(ApplicationDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var emailExists = await _db.AppUsers
            .AsNoTracking()
            .AnyAsync(u => u.Email == dto.Email);

        if (emailExists)
            throw new InvalidOperationException("Email already exists.");

        var usernameExists = await _db.AppUsers
            .AsNoTracking()
            .AnyAsync(u => u.Username == dto.Username);

        if (usernameExists)
            throw new InvalidOperationException("Username already exists.");

        await using var transaction = await _db.Database.BeginTransactionAsync();

        var user = new AppUser
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };

        _db.AppUsers.Add(user);
        await _db.SaveChangesAsync();

        if (dto.Role == UserRole.Doctor)
        {
            var existingDoctor = await _db.Doctors
                .AsNoTracking()
                .AnyAsync(i => i.Email == dto.Email);

            if (!existingDoctor)
            {
                var doctor = new Doctor
                {
                    FirstName = dto.Username,
                    LastName = "User",
                    Email = dto.Email,
                    Department = "General"
                };

                _db.Doctors.Add(doctor);
                await _db.SaveChangesAsync();
            }
        }
        else if (dto.Role == UserRole.Patient)
        {
            var existingPatient = await _db.Patients
                .AsNoTracking()
                .AnyAsync(s => s.Email == dto.Email);

            if (!existingPatient)
            {
                var patient = new Patient
                {
                    FirstName = dto.Username,
                    LastName = "User",
                    Email = dto.Email,
                    PatientNumber = $"PAT{user.Id:D6}",
                    DateOfBirth = DateTime.UtcNow.AddYears(-18)
                };

                _db.Patients.Add(patient);
                await _db.SaveChangesAsync();
            }
        }

        await transaction.CommitAsync();

        return new AuthResponseDto
        {
            Token = GenerateToken(user),
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            ExpiresAt = DateTime.UtcNow.AddHours(GetExpiryHours())
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _db.AppUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        return new AuthResponseDto
        {
            Token = GenerateToken(user),
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            ExpiresAt = DateTime.UtcNow.AddHours(GetExpiryHours())
        };
    }

    private string GenerateToken(AppUser user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(GetExpiryHours()),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private double GetExpiryHours() =>
        double.TryParse(_config["Jwt:ExpiryHours"], out var h) ? h : 24;
}
