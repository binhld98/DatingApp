using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(8)]
    public string Password { get; set; }
}
