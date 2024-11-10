using me.vldf.jsa.dsl.ir.nodes.declarations;

namespace me.vldf.jsa.dsl.ast.types;

public class ObjectAstType(ObjectAstNode objectNode) : AstType
{
    public override string Name { get; } = objectNode.Name;
    public override string String()
    {
        return $"object({objectNode.Name})";
    }
}
