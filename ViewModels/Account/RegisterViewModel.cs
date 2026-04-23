using System.ComponentModel.DataAnnotations;

namespace E_PANEL.ViewModels.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [Display(Name = "Nome completo")]
    [MaxLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória.")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmação de senha é obrigatória.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar senha")]
    [Compare(nameof(Password), ErrorMessage = "As senhas não conferem.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Número da ETEC é obrigatório.")]
    [Display(Name = "Número da ETEC")]
    [MaxLength(20)]
    public string EtecNumber { get; set; } = string.Empty;

    [Display(Name = "Cadastrar como professor")]
    public bool IsProfessor { get; set; }
}
