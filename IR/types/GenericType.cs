using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.types;

public class GenericType(
    string name) : AstType
{
    public override string Name => name;
    public override string String()
    {
        return Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is GenericType gen && Equals(gen);
    }

    protected bool Equals(GenericType other)
    {
        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }

    public override TypeReference GetReference(IrContext context) => new(Name, context);
}
