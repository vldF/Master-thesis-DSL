using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

 public class FunctionArgAstNode(string name, TypeReference typeRef, int index) : VarDeclAstNode(name, typeRef, null)
{
    public int Index { get; } = index;

    public override string String()
    {
        return $"arg {Name}: {TypeReference.AsString()}";
    }
}
