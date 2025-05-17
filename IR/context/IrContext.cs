using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.context;

public class IrContext
{
    public string? Package { get; set; }
    public IAstNode? AstNode { get; set; }
    private readonly IrContext? _parent;
    private readonly Dictionary<string, VarDeclAstNode> _vars = new ();
    private readonly Dictionary<string, FunctionArgAstNode> _args = new ();
    private readonly Dictionary<string, FunctionAstNodeBase> _funcs = new ();
    private readonly Dictionary<string, ObjectAstNode> _objects = new ();
    private readonly Dictionary<string, AstType> _types = new();
    private readonly List<string> _importNames = [];
    private readonly List<IrContext> _imports = [];

    public readonly TypeReference AnyTypeRef;

    public IrContext(
        IrContext? parent,
        string? package = null)
    {
        _parent = parent;
        Package = package;
        AnyTypeRef = new TypeReference("any", this);

        var noneVar = new VarDeclAstNode("none", AnyTypeRef, null)
        {
            IsNone = true
        };
        SaveNewVar(noneVar);
    }

    public void SaveNewVar(VarDeclAstNode node)
    {
        _vars[node.Name] = node;
    }

    public void SaveNewArg(FunctionArgAstNode node)
    {
        _args[node.Name] = node;
    }

    public void SaveNewFunc(FunctionAstNodeBase node)
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

    public void SaveImport(string package)
    {
        if (_importNames.Contains(package))
        {
            return;
        }

        _importNames.Add(package);
    }

    public void InitializeImports(IReadOnlyCollection<IrContext> contexts)
    {
        foreach (var irContext in contexts)
        {
            var irContextPackage = irContext.Package;
            if (irContextPackage == null)
            {
                continue;
            }

            if (_importNames.Contains(irContextPackage))
            {
                _imports.Add(irContext);
            }
        }
    }

    public VarDeclAstNode? ResolveVar(string id, IReadOnlyCollection<IrContext>? visited = null)
    {
        visited ??= [];
        var newVisited = visited.Append(this).ToList();

        return _vars.GetValueOrDefault(id) ?? _args.GetValueOrDefault(id)
               ?? _parent?.ResolveVar(id, newVisited)
               ?? ResolveOverImports(ctx => ctx.ResolveVar(id, newVisited), newVisited);
    }

    public FunctionAstNodeBase? ResolveFunc(string id, IReadOnlyCollection<IrContext>? visited = null)
    {
        visited ??= [];
        var newVisited = visited.Append(this).ToList();

        return _funcs.GetValueOrDefault(id)
               ?? _parent?.ResolveFunc(id, newVisited)
               ?? ResolveOverImports(ctx => ctx.ResolveFunc(id, newVisited), newVisited);
    }

    public ObjectAstNode? ResolveObject(string id, IReadOnlyCollection<IrContext>? visited = null)
    {
        visited ??= [];
        var newVisited = visited.Append(this).ToList();

        return _objects.GetValueOrDefault(id)
               ?? _parent?.ResolveObject(id, newVisited)
               ?? ResolveOverImports(ctx => ctx.ResolveObject(id, newVisited), newVisited);
    }

    public AstType? ResolveType(string id, IReadOnlyCollection<IrContext>? visited = null)
    {
        visited ??= [];
        var newVisited = visited.Append(this).ToList();

        return _types.GetValueOrDefault(id)
               ?? _parent?.ResolveType(id, newVisited)
               ?? ResolveOverImports(ctx => ctx.ResolveType(id, newVisited), newVisited);
    }

    private T? ResolveOverImports<T>(Func<IrContext, T?> resolver, IReadOnlyCollection<IrContext>? visited = null)
    {
        foreach (var irContext in _imports)
        {
            if (visited?.Contains(irContext) == true)
            {
                continue;
            }

            var resOrNull = resolver(irContext);

            if (resOrNull != null)
            {
                return resOrNull;
            }
        }

        return default;
    }
}
