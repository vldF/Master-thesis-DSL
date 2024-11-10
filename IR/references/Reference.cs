using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using TypeReference = me.vldf.jsa.dsl.ir.references.Reference<me.vldf.jsa.dsl.ast.types.AstType>;
using FunctionReference = me.vldf.jsa.dsl.ir.references.Reference<me.vldf.jsa.dsl.ir.nodes.declarations.FunctionAstNode>;
using ObjectReference = me.vldf.jsa.dsl.ir.references.Reference<me.vldf.jsa.dsl.ir.nodes.declarations.ObjectAstNode>;
using VariableReference = me.vldf.jsa.dsl.ir.references.Reference<me.vldf.jsa.dsl.ir.nodes.declarations.VarDeclAstNode>;

namespace me.vldf.jsa.dsl.ir.references;

public abstract class Reference<T>()
{
    public abstract T? Resolve();

    public abstract string AsString();
    public T? SealedValue { get; set; } = default;

    public R Clone<R>() where R : Reference<T>
    {
        return (R)this.MemberwiseClone();
    }
}

public class TypeReference(string id, IrContext context) : Reference<AstType>()
{
    public override AstType? Resolve()
    {
        if (SealedValue != null)
        {
            return SealedValue;
        }

        return context.ResolveType(id);
    }

    public override string AsString() => $"TypeRef[{id}]";
}

public class FunctionReference(string id, IrContext context) : Reference<FunctionAstNode>()
{
    public override FunctionAstNode? Resolve()
    {
        if (SealedValue != null)
        {
            return SealedValue;
        }

        return context.ResolveFunc(id);
    }

    public override string AsString() => $"FunctionRef[{id}]";
}

public class ObjectReference(string id, IrContext context) : Reference<ObjectAstNode>()
{
    public override ObjectAstNode? Resolve()
    {
        if (SealedValue != null)
        {
            return SealedValue;
        }

        return context.ResolveObject(id);
    }

    public override string AsString() => $"ObjectRef[{id}]";
}

public class VariableReference(string Id, IrContext context) : Reference<VarDeclAstNode>()
{
    public override VarDeclAstNode? Resolve()
    {
        if (SealedValue != null)
        {
            return SealedValue;
        }

        return context.ResolveVar(Id);
    }

    public override string AsString() => $"VariableRef[{Id}]";
}
