using System.ComponentModel.DataAnnotations;

namespace Gymly.Web.Models.Users;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, MinimumLength = 3)]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full name is required.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required.")]
    [Display(Name = "Role")]
    public int SystemRoleId { get; set; }

    public List<RoleOption> AvailableRoles { get; set; } = [];
}

public class RoleOption
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}