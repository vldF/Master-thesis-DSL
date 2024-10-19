namespace me.vldf.jsa.dsl.ast.types;

public class SimpleAstType(string name) : AstType
{
    public override string Name { get; } = name;

    public override string String()
    {
        return $"Type({Name})";
    }
}
