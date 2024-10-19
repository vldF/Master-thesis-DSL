namespace Codegen.IR.nodes.types;

public class CgSimpleType(string name) : ICgType
{
    public string Name { get; } = name;

    public static CgSimpleType IntType = new("int");
    public static CgSimpleType StringType = new("string");
    public static CgSimpleType VoidType = new("void");
}
