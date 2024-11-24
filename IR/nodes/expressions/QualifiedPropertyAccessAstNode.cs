namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public class QualifiedAccessPropertyAstNode(
    IExpressionAstNode parent,
    string propertyName
    ) : QualifiedAccessAstNodeBase(parent)
{
    public string PropertyName { get; } = propertyName;

    public override string String()
    {
        return $"({Parent.String()}).{PropertyName}";
    }
}
