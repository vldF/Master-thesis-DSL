namespace me.vldf.jsa.dsl.ast.nodes;

public abstract class AstNode
{
    public abstract string String();

    protected string AddIndent(string str)
    {
        var indent = "  ";
        return string.Join("\n", str.Split("\n").Select(line => indent + line));
    }
}
