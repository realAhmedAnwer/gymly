using System.ComponentModel.DataAnnotations;

namespace Gymly.Web.Models.Trainers;

public class CreateTrainerViewModel
{
    [Required(ErrorMessage = "Trainer name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address formatting.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^0[0125]\d{9}$", ErrorMessage = "Phone must be a valid Egyptian number (e.g. 011XXXXXXXX).")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Specialization field is required.")]
    [Display(Name = "Specialization / Focus")]
    public string Specialization { get; set; } = string.Empty;
}