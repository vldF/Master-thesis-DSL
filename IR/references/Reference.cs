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
    public abstract T? Resolve(IrContext? ctx = null);

    public abstract string AsString();
    public T? SealedValue { get; set; } = default;

    public abstract string Name { get; set; }

    public R Clone<R>() where R : Reference<T>
    {
        return (R)this.MemberwiseClone();
    }
}

public class TypeReference(string id, IrContext context) : Reference<AstType>()
{
    public override AstType? Resolve(IrContext? ctx = null)
    {
        if (SealedValue != null)
        {
            return SealedValue;
        }

        return ctx?.ResolveType(id) ?? context.ResolveType(id);
    }

    public ObjectAstNode? ResolveObject(IrContext? ctx = null)
    {
        return ctx?.ResolveObject(id) ?? context.ResolveObject(id);
    }

    public override string AsString() => $"TypeRef[{id}]";
    public override string Name { get; set; } = id;
}

public class FunctionReference(string id, IrContext context) : Reference<FunctionAstNodeBase>()
{
    public override FunctionAstNodeBase? Resolve(IrContext? ctx = null)
    {
        if (SealedValue != null)
        {
            return SealedValue;
        }

        return ctx?.ResolveFunc(id) ?? context.ResolveFunc(id);
    }

    public override string AsString() => $"FunctionRef[{id}]";
    public override string Name { get; set; } = id;
}

public class ObjectReference(string id, IrContext context) : Reference<ObjectAstNode>()
{
    public override ObjectAstNode? Resolve(IrContext? ctx = null)
    {
        if (SealedValue != null)
        {
            return SealedValue;
        }

        return ctx?.ResolveObject(id) ?? context.ResolveObject(id);
    }

    public override string AsString() => $"ObjectRef[{id}]";
    public override string Name { get; set; } = id;
}

public class VariableReference(string Id, IrContext context) : Reference<VarDeclAstNode>()
{
    public readonly IrContext Context = context;

    public override VarDeclAstNode? Resolve(IrContext? ctx = null)
    {
        if (SealedValue != null)
        {
            return SealedValue;
        }

        return Context.ResolveVar(Id);
    }

    public override string AsString() => $"VariableRef[{Id}]";
    public override string Name { get; set; } = Id;
}
