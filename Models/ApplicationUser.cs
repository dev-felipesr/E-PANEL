using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace E_PANEL.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string EtecNumber { get; set; } = string.Empty;
}
