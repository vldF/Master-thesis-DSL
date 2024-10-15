namespace me.vldf.jsa.dsl.ast.types;

public class SimpleType(string name) : Type
{
    public override string Name { get; } = name;

    public override string String()
    {
        return $"Type({Name})";
    }
}
