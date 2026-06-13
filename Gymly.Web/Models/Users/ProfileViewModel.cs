using System.ComponentModel.DataAnnotations;

namespace Gymly.Web.Models.Users;

public class ProfileViewModel
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full name is required.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}