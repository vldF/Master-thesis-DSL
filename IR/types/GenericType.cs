using me.vldf.jsa.dsl.ast.types;

namespace me.vldf.jsa.dsl.ir.types;

public class GenericType(
    string name) : AstType
{
    public override string Name => name;
    public override string String()
    {
        return Name;
    }
}
