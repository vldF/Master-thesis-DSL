using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ast.types;

public class ObjectAstType(ObjectAstNode objectNode) : AstType
{
    public override string Name { get; } = objectNode.Name;
    public override string String()
    {
        return $"object({objectNode.Name})";
    }

    public override TypeReference GetReference(IrContext context) => new(Name, context);
}
