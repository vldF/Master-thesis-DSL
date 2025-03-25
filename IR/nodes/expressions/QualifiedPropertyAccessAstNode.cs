namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public class QualifiedAccessPropertyAstNode(
    IExpressionAstNode qualifiedParent,
    string propertyName
    ) : QualifiedAccessAstNodeBase(qualifiedParent)
{
    public string PropertyName { get; } = propertyName;

    public override string String()
    {
        return $"({QualifiedParent?.String()}).{PropertyName}";
    }

    public new bool IsSyntetic { get; set; }
}
