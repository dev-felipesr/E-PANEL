using System.ComponentModel.DataAnnotations;
using E_PANEL.Models;

namespace E_PANEL.ViewModels.Posts;

public class CreatePostViewModel
{
    [Required(ErrorMessage = "Título é obrigatório.")]
    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Conteúdo é obrigatório.")]
    [MaxLength(2500)]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Tipo de postagem")]
    public PostType PostType { get; set; } = PostType.Regular;
}
