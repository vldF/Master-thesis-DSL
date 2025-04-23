using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.nodes;

public record AnnotationAstNode(string Name, ICollection<IExpressionAstNode> args) : IAstNode
{
    public string String()
    {
        return $"@{Name}({string.Join(", ", args)})";
    }
}
