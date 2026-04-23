namespace E_PANEL.Models;

public enum PostType
{
    [System.ComponentModel.DataAnnotations.Display(Name = "Post da comunidade")]
    Regular = 0,
    [System.ComponentModel.DataAnnotations.Display(Name = "Aviso")]
    Announcement = 1,
    [System.ComponentModel.DataAnnotations.Display(Name = "Evento")]
    Event = 2
}
