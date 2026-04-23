namespace E_PANEL.Services;

public interface IProfanityFilter
{
    bool ContainsProfanity(string text);
    string Sanitize(string text);
}
