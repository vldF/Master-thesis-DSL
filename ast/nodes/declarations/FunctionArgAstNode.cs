using Type = me.vldf.jsa.dsl.ast.types.Type;

namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class FunctionArgAstNode(string name, Type type) : VarDeclAstNode(name, type, null)
{
    public override string String()
    {
        return $"arg {Name}: {Type.String()}";
    }
}
