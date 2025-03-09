using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

public class IntrinsicFunctionAstNode(
    string name,
    IReadOnlyCollection<FunctionArgAstNode> args,
    TypeReference returnTypeRef,
    ObjectAstNode? parent) : FunctionAstNodeBase(name, args, returnTypeRef, parent)
{
    public override string String()
    {
        var argsAsString = string.Join(", ", Args.Select(x => x.String()));
        var returnTypeAsStr = ReturnTypeRef != null ? ": " + ReturnTypeRef.AsString() : "";
        return $"intrinsic func {Name} ({argsAsString}) {returnTypeAsStr}";
    }
}
