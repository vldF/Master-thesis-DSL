using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

public class VarDeclAstNode(
    string name,
    TypeReference? typeRef,
    IExpressionAstNode? init) : IStatementAstNode
{
    public IAstNode? Parent { get; set; } = null;

    public IExpressionAstNode? Init = init;

    public string Name { get; } = name;
    public TypeReference? TypeReference { get; set; } = typeRef;

    public virtual string String()
    {
        if (Init != null)
        {
            return $"@{Name}: @{TypeReference?.AsString() ?? "<unresolved>"} = {Init.String()}";
        }

        return $"@{Name}: @{TypeReference?.AsString() ?? "<unresolved>"}";
    }

    public bool IsObjectField()
    {
        return Parent is ObjectAstNode;
    }

    public bool IsSelf { get; init; } = false;

    public bool IsLocalVarDecl => !IsObjectField();

    public bool IsNone { get; init; } = false;
}
