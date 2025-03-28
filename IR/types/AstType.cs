using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ast.types;

public abstract class AstType
{
    public abstract string Name { get; }
    public abstract string String();
    public abstract TypeReference GetReference(IrContext context);
}
