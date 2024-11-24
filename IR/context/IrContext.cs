using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.context;

public class IrContext
{
    private readonly IrContext? _parent;
    private readonly Dictionary<string, VarDeclAstNode> _vars = new ();
    private readonly Dictionary<string, FunctionArgAstNode> _args = new ();
    private readonly Dictionary<string, FunctionAstNode> _funcs = new ();
    private readonly Dictionary<string, ObjectAstNode> _objects = new ();
    private readonly Dictionary<string, AstType> _types = new();

    public readonly TypeReference AnyTypeRef;

    public IrContext(IrContext? parent)
    {
        _parent = parent;
        AnyTypeRef = new TypeReference("any", this);
    }

    public void SaveNewVar(VarDeclAstNode node)
    {
        _vars[node.Name] = node;
    }

    public void SaveNewArg(FunctionArgAstNode node)
    {
        _args[node.Name] = node;
    }

    public void SaveNewFunc(FunctionAstNode node)
    {
        _funcs[node.Name] = node;
    }

    public void SaveNewObject(ObjectAstNode node)
    {
        _objects[node.Name] = node;
    }

    public void SaveNewType(AstType astType)
    {
        _types[astType.Name] = astType;
    }


    public VarDeclAstNode? ResolveVar(string id)
    {
        return _vars.GetValueOrDefault(id) ?? (_args.GetValueOrDefault(id) ?? _parent?.ResolveVar(id));
    }

    public FunctionAstNode? ResolveFunc(string id)
    {
        return _funcs.GetValueOrDefault(id) ?? _parent?.ResolveFunc(id);
    }

    public ObjectAstNode? ResolveObject(string id)
    {
        return _objects.GetValueOrDefault(id) ?? _parent?.ResolveObject(id);
    }

    public AstType? ResolveType(string id)
    {
        return _types.GetValueOrDefault(id) ?? _parent?.ResolveType(id);
    }
}
