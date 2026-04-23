using System.Text.RegularExpressions;

namespace E_PANEL.Services;

public class ProfanityFilter : IProfanityFilter
{
    private static readonly string[] ForbiddenWords =
    [
        "porra",
        "caralho",
        "merda",
        "bosta",
        "puta",
        "foda",
        "fdp",
        "desgraca",
        "desgraça"
    ];

    public bool ContainsProfanity(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        return ForbiddenWords.Any(word => WordRegex(word).IsMatch(text));
    }

    public string Sanitize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        var sanitized = text;
        foreach (var word in ForbiddenWords)
        {
            sanitized = WordRegex(word).Replace(sanitized, match => new string('*', match.Length));
        }

        return sanitized;
    }

    private static Regex WordRegex(string word)
    {
        var pattern = $@"\b{Regex.Escape(word)}\b";
        return new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }
}
