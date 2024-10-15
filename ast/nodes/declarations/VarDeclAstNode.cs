using me.vldf.jsa.dsl.ast.nodes.expressions;

namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class VarDeclAstNode(
    string name,
    string? type,
    ExpressionAstNode? init) : AstNode
{
    public string Name { get; } = name;
    public string Type { get; } = type;

    public override string String()
    {
        if (init != null)
        {
            return $"{name}: {type} = {init.String()}";
        }

        return $"{name}: {type}";
    }
}
