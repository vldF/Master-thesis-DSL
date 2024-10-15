using me.vldf.jsa.dsl.ast.nodes.declarations;

namespace me.vldf.jsa.dsl.ast.types;

public class ObjectType(ObjectAstNode objectNode) : Type
{
    public override string Name { get; } = objectNode.Name;
    public override string String()
    {
        return $"object({objectNode.Name})";
    }
}
