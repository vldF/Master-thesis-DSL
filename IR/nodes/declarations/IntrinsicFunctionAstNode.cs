using me.vldf.jsa.dsl.ir.references;
using me.vldf.jsa.dsl.ir.types;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

public class IntrinsicFunctionAstNode(
    string name,
    IReadOnlyCollection<FunctionArgAstNode> args,
    IReadOnlyCollection<GenericType> generics,
    TypeReference returnTypeRef,
    ObjectAstNode? parent) : FunctionAstNodeBase(name, args, returnTypeRef, parent)
{
    public override string String()
    {
        var argsAsString = string.Join(", ", Args.Select(x => x.String()));
        var returnTypeAsStr = ReturnTypeRef != null ? ": " + ReturnTypeRef.AsString() : "";
        var genericsString = generics.Count > 0 ?  $"<{string.Join(", ", generics.Select(g => g.Name))}>": "";
        return $"intrinsic func {Name}{genericsString}({argsAsString}) {returnTypeAsStr}";
    }
}
