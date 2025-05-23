using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

public class FunctionAstNode(
    string name,
    IReadOnlyCollection<FunctionArgAstNode> args,
    TypeReference returnTypeRef,
    StatementsBlockAstNode body,
    ObjectAstNode? parent,
    ICollection<AnnotationAstNode> annotations,
    IrContext context) : FunctionAstNodeBase(name, args, returnTypeRef, parent), IContextOwner
{
    public ICollection<AnnotationAstNode> Annotations { get; } = annotations;
    public IrContext Context { get; set; } = context;

    public StatementsBlockAstNode Body { get; set; } = body;

    public override string String()
    {
        var argsAsString = string.Join(", ", Args.Select(x => x.String()));
        var returnTypeAsStr = ReturnTypeRef != null ? ": " + ReturnTypeRef.AsString() : "";
        var annos = string.Join(", ", Annotations.Select(a => a.String()));
        return $$"""
                {{annos}}
                func {{Name}} ({{argsAsString}}) {{returnTypeAsStr}} {
                {{AddIndent(Body.String())}}
                }
                """;
    }
}
