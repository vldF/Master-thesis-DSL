namespace TestPlatform;

public static class StringUtils
{
    public static string EnsureTrailingNewLine(this string text)
    {
        if (text.EndsWith($"\n"))
        {
            return text;
        }

        return text + "\n";
    }
}
