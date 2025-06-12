using System.ComponentModel.DataAnnotations;

namespace Korrekturmanagementsystem.Dtos;

public class CreateUserDto
{
    [Required(ErrorMessage = "Benutzername ist erforderlich.")]
    [StringLength(50, ErrorMessage = "Benutzername darf maximal 50 Zeichen enthalten.")]
    public string Username { get; set; } = default!;

    [Required(ErrorMessage = "E-Mail ist erforderlich.")]
    [EmailAddress(ErrorMessage = "Bitte eine gültige E-Mail-Adresse eingeben.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Passwort ist erforderlich.")]
    [MinLength(6, ErrorMessage = "Das Passwort muss mindestens 6 Zeichen lang sein.")]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "Bitte eine Rolle auswählen.")]
    public Guid StakeholderRoleId { get; set; }
}
