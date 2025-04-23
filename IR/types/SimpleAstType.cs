using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ast.types;

public class SimpleAstType(string name) : AstType
{
    public static SimpleAstType Int = new("int");
    public static SimpleAstType Any = new("any");
    public static SimpleAstType StringT = new("string");
    public static SimpleAstType Bool = new("bool");
    public static SimpleAstType Float = new("float");
    public static SimpleAstType Bytes = new("bytes");

    public override string Name { get; } = name;

    public override string String()
    {
        return $"Type({Name})";
    }

    public override TypeReference GetReference(IrContext context) => new(Name, context);
}
