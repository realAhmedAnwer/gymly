using System.ComponentModel.DataAnnotations;

namespace Gymly.Web.Models.Members;

public class CreateMemberViewModel
{
    [Required(ErrorMessage = "Member name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [RegularExpression(@"^$|^0[0125]\d{9}$", ErrorMessage = "Phone must be a valid Egyptian number (e.g. 011XXXXXXXX).")]
    public string Phone { get; set; } = string.Empty;
}