namespace me.vldf.jsa.dsl.ast.types;

public class SimpleAstType(string name) : AstType
{
    public static SimpleAstType Int = new("int");
    public static SimpleAstType Any = new("any");
    public static SimpleAstType StringT = new("string");
    public static SimpleAstType Bool = new("bool");
    public static SimpleAstType Float = new("float");

    public override string Name { get; } = name;

    public override string String()
    {
        return $"Type({Name})";
    }
}
