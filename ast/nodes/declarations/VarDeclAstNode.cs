using me.vldf.jsa.dsl.ast.nodes.expressions;

namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class VarDeclAstNode(
    string name,
    types.Type type,
    ExpressionAstNode? init) : AstNode
{
    public string Name { get; } = name;
    public types.Type Type { get; } = type;

    public override string String()
    {
        if (init != null)
        {
            return $"{Name}: {Type.String()} = {init.String()}";
        }

        return $"{Name}: {Type.String()}";
    }
}
