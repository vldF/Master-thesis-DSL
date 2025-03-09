using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

public abstract class FunctionAstNodeBase(
    string name,
    IReadOnlyCollection<FunctionArgAstNode> args,
    TypeReference returnTypeRef,
    ObjectAstNode? parent) : IStatementAstNode
{
    public string Name { get; set; } = name;

    public List<FunctionArgAstNode> Args { get; set; } = args.ToList();
    public TypeReference? ReturnTypeRef { get; set; } = returnTypeRef;
    public IAstNode? Parent { get; set; } = parent;

    public abstract string String();
}
