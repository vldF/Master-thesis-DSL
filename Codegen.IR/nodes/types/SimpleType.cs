namespace Codegen.IR.nodes.types;

public class SimpleType(string name) : ICgType
{
    public string Name { get; } = name;

    public static SimpleType IntType = new("int");
    public static SimpleType StringType = new("string");
}
