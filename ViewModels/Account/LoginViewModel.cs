using System.ComponentModel.DataAnnotations;

namespace E_PANEL.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email é obrigatório.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Número da ETEC é obrigatório.")]
    [Display(Name = "Número da ETEC")]
    [MaxLength(20)]
    public string EtecNumber { get; set; } = string.Empty;

    [Display(Name = "Lembrar acesso")]
    public bool RememberMe { get; set; }
}
