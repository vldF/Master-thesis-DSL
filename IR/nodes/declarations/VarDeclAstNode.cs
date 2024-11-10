using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

public class VarDeclAstNode(
    string name,
    TypeReference typeRef,
    IExpressionAstNode? init) : IStatementAstNode
{
    public string Name { get; } = name;
    public TypeReference TypeReference { get; set; } = typeRef;

    public virtual string String()
    {
        if (init != null)
        {
            return $"{Name}: @{TypeReference.AsString()} = {init.String()}";
        }

        return $"{Name}: @{TypeReference.AsString()}";
    }
}
