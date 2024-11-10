global using static me.vldf.jsa.dsl.ir.nodes.Utils;
namespace me.vldf.jsa.dsl.ir.nodes;

public static class Utils
{
    public static string AddIndent(string str)
    {
        var indent = "  ";
        return string.Join("\n", str.Split("\n").Select(line => indent + line));
    }

    public static T Copy<T>(this IAstNode node) where T : IAstNode
    {
        var inst = node.GetType().GetMethod("MemberwiseClone",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        return (T)inst!.Invoke(node, null)!;
    }
}
