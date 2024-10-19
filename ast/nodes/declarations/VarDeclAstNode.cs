using me.vldf.jsa.dsl.ast.nodes.expressions;
using me.vldf.jsa.dsl.ast.nodes.statements;
using me.vldf.jsa.dsl.ast.types;

namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class VarDeclAstNode(
    string name,
    AstType type,
    IExpressionAstNode? init) : IStatementAstNode
{
    public string Name { get; } = name;
    public AstType Type { get; } = type;

    public virtual string String()
    {
        if (init != null)
        {
            return $"{Name}: {Type.String()} = {init.String()}";
        }

        return $"{Name}: {Type.String()}";
    }
}
