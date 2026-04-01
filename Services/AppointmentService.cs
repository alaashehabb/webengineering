using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Appointment;
using CourseManagementAPI.Interfaces;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public class AppointmentService : IAppointmentService
{
    private readonly ApplicationDbContext _db;

    public AppointmentService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync()
    {
        return await _db.Appointments
            .AsNoTracking()
            .Select(e => new AppointmentResponseDto
            {
                Id = e.Id,
                PatientId = e.PatientId,
                PatientFullName = e.Patient.FirstName + " " + e.Patient.LastName,
                MedicalServiceId = e.MedicalServiceId,
                MedicalServiceTitle = e.MedicalService.Title,
                Status = e.Status,
                Grade = e.Grade,
                ScheduledAt = e.ScheduledAt
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByPatientAsync(int patientId)
    {
        return await _db.Appointments
            .AsNoTracking()
            .Where(e => e.PatientId == patientId)
            .Select(e => new AppointmentResponseDto
            {
                Id = e.Id,
                PatientId = e.PatientId,
                PatientFullName = e.Patient.FirstName + " " + e.Patient.LastName,
                MedicalServiceId = e.MedicalServiceId,
                MedicalServiceTitle = e.MedicalService.Title,
                Status = e.Status,
                Grade = e.Grade,
                ScheduledAt = e.ScheduledAt
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByMedicalServiceAsync(int medicalServiceId)
    {
        return await _db.Appointments
            .AsNoTracking()
            .Where(e => e.MedicalServiceId == medicalServiceId)
            .Select(e => new AppointmentResponseDto
            {
                Id = e.Id,
                PatientId = e.PatientId,
                PatientFullName = e.Patient.FirstName + " " + e.Patient.LastName,
                MedicalServiceId = e.MedicalServiceId,
                MedicalServiceTitle = e.MedicalService.Title,
                Status = e.Status,
                Grade = e.Grade,
                ScheduledAt = e.ScheduledAt
            })
            .ToListAsync();
    }

    public async Task<AppointmentResponseDto?> GetByIdAsync(int id)
    {
        return await _db.Appointments
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new AppointmentResponseDto
            {
                Id = e.Id,
                PatientId = e.PatientId,
                PatientFullName = e.Patient.FirstName + " " + e.Patient.LastName,
                MedicalServiceId = e.MedicalServiceId,
                MedicalServiceTitle = e.MedicalService.Title,
                Status = e.Status,
                Grade = e.Grade,
                ScheduledAt = e.ScheduledAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto)
    {
        var patientExists = await _db.Patients
            .AsNoTracking()
            .AnyAsync(s => s.Id == dto.PatientId);

        if (!patientExists)
            throw new KeyNotFoundException("Patient not found.");

        var medicalServiceExists = await _db.MedicalServices
            .AsNoTracking()
            .AnyAsync(c => c.Id == dto.MedicalServiceId);

        if (!medicalServiceExists)
            throw new KeyNotFoundException("Medical service not found.");

        var appointmentExists = await _db.Appointments
            .AsNoTracking()
            .AnyAsync(e => e.PatientId == dto.PatientId && e.MedicalServiceId == dto.MedicalServiceId);

        if (appointmentExists)
            throw new InvalidOperationException("Patient already has an appointment for this medical service.");

        var appointment = new Appointment
        {
            PatientId = dto.PatientId,
            MedicalServiceId = dto.MedicalServiceId
        };

        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();

        await _db.Entry(appointment).Reference(e => e.Patient).LoadAsync();
        await _db.Entry(appointment).Reference(e => e.MedicalService).LoadAsync();

        return new AppointmentResponseDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            PatientFullName = appointment.Patient.FirstName + " " + appointment.Patient.LastName,
            MedicalServiceId = appointment.MedicalServiceId,
            MedicalServiceTitle = appointment.MedicalService.Title,
            Status = appointment.Status,
            Grade = appointment.Grade,
            ScheduledAt = appointment.ScheduledAt
        };
    }

    public async Task<AppointmentResponseDto?> UpdateAsync(int id, UpdateAppointmentDto dto)
    {
        var appointment = await _db.Appointments
            .Include(e => e.Patient)
            .Include(e => e.MedicalService)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (appointment is null) return null;

        appointment.Status = dto.Status;
        appointment.Grade = dto.Grade;

        await _db.SaveChangesAsync();

        return new AppointmentResponseDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            PatientFullName = appointment.Patient.FirstName + " " + appointment.Patient.LastName,
            MedicalServiceId = appointment.MedicalServiceId,
            MedicalServiceTitle = appointment.MedicalService.Title,
            Status = appointment.Status,
            Grade = appointment.Grade,
            ScheduledAt = appointment.ScheduledAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var appointment = await _db.Appointments.FindAsync(id);
        if (appointment is null) return false;

        _db.Appointments.Remove(appointment);
        await _db.SaveChangesAsync();
        return true;
    }
}
