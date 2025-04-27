using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

 public class FunctionArgAstNode(
     string name,
     TypeReference typeRef,
     int index,
     IExpressionAstNode? defaultValue = null) : VarDeclAstNode(name, typeRef, null)
{
    public int Index { get; set; } = index;
    public IExpressionAstNode? DefaultValue { get; set; } = defaultValue;

    public override string String()
    {
        return $"arg[{Name}]: {TypeReference?.AsString() ?? "<unresolved>"}";
    }
}
