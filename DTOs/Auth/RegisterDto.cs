using System.ComponentModel.DataAnnotations;
using CourseManagementAPI.Models;

namespace CourseManagementAPI.DTOs.Auth;

public class RegisterDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Patient;
}
