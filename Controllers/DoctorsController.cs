using CourseManagementAPI.DTOs.Doctor;
using CourseManagementAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

// 1. We add this small class so .NET understands the incoming React data
public class DoctorRequest
{
    public string Name { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private static List<dynamic> _mockDoctors = new List<dynamic>
    {
        new { Id = 1, Name = "Dr. Smith", Specialization = "Cardiology" },
        new { Id = 2, Name = "Dr. Sarah", Specialization = "Neurology" },
        new { Id = 3, Name = "Dr. Ahmed", Specialization = "Pediatrics" }
    };

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_mockDoctors);
    }

    // 2. We use DoctorRequest here instead of 'dynamic'
    [HttpPost]
    public IActionResult Create([FromBody] DoctorRequest data)
    {
        try 
        {
            // Logic to generate the next ID
            int newId = _mockDoctors.Count > 0 ? _mockDoctors.Max(d => (int)d.Id) + 1 : 1;

            // Map the data correctly
            var newDoctor = new
            {
                Id = newId,
                Name = data.Name,
                Specialization = data.Specialization
            };

            _mockDoctors.Add(newDoctor);
            return Ok(newDoctor);
        }
        catch (Exception ex)
        {
            // This helps you see the real error in the terminal if it still fails
            Console.WriteLine("Add Error: " + ex.Message);
            return BadRequest("Could not add doctor");
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var doctor = _mockDoctors.FirstOrDefault(d => d.Id == id);
        
        if (doctor == null)
        {
            return NotFound();
        }

        _mockDoctors.Remove(doctor);
        return NoContent();
    }
}