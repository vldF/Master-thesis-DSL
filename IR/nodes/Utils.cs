global using static me.vldf.jsa.dsl.ast.nodes.Utils;
namespace me.vldf.jsa.dsl.ast.nodes;

public static class Utils
{
    public static string AddIndent(string str)
    {
        var indent = "  ";
        return string.Join("\n", str.Split("\n").Select(line => indent + line));
    }
}
