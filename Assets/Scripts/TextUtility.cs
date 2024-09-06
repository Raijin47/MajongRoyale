public static class TextUtility
{
    private static readonly string[] color = 
    { 
        "#F1B113",
        "#0CC3DC",
        "#F6D909",
        "#F3F2A3"
    };

    public static string GetColorText(string text, int id)
    {
        return $"<color={color[id]}>{text}</color>";
    }
}