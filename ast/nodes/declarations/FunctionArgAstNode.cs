using me.vldf.jsa.dsl.ast.types;

namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class FunctionArgAstNode(string name, AstType type) : VarDeclAstNode(name, type, null)
{
    public override string String()
    {
        return $"arg {Name}: {Type.String()}";
    }
}
