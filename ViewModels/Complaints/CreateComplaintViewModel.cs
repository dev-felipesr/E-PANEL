using System.ComponentModel.DataAnnotations;

namespace E_PANEL.ViewModels.Complaints;

public class CreateComplaintViewModel
{
    [Required(ErrorMessage = "Título é obrigatório.")]
    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória.")]
    [MaxLength(2500)]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Publicar como anônimo")]
    public bool IsAnonymous { get; set; }
}
