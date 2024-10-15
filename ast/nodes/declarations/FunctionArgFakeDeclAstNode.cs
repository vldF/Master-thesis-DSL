namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class FunctionArgFakeDeclAstNode(string name, string? type) : VarDeclAstNode(name, type, null)
{
    public override string String()
    {
        return $"arg {Name}: {Type}";
    }
}
