using me.vldf.jsa.dsl.ast.types;

namespace me.vldf.jsa.dsl.ast.nodes.declarations;

 public class FunctionArgAstNode(string name, AstType type, int index) : VarDeclAstNode(name, type, null)
{
    public string Name { get; } = name;
    public AstType Type { get; } = type;
    public int Index { get; } = index;

    public override string String()
    {
        return $"arg {Name}: {Type.String()}";
    }
}
