using me.vldf.jsa.dsl.ast.nodes.declarations;
using me.vldf.jsa.dsl.ast.types;
using Type = me.vldf.jsa.dsl.ast.types.Type;

namespace me.vldf.jsa.dsl.ast.context;

public class AstContext(AstContext? parent)
{
    private readonly Dictionary<string, VarDeclAstNode> _vars = new ();
    private readonly Dictionary<string, FunctionAstNode> _funcs = new ();
    private readonly Dictionary<string, ObjectAstNode> _objects = new ();
    private readonly Dictionary<string, Type> _types = new();

    public readonly Type AnyType = new SimpleType("any");

    public void SaveNewVar(VarDeclAstNode node)
    {
        _vars[node.Name] = node;
    }

    public void SaveNewFunc(FunctionAstNode node)
    {
        _funcs[node.Name] = node;
    }

    public void SaveNewObject(ObjectAstNode node)
    {
        _objects[node.Name] = node;
    }

    public void SaveNewType(Type type)
    {
        _types[type.Name] = type;
    }


    public VarDeclAstNode? ResolveVar(string name)
    {
        return _vars.GetValueOrDefault(name) ?? parent?.ResolveVar(name);
    }

    public FunctionAstNode? ResolveFunc(string name)
    {
        return _funcs.GetValueOrDefault(name) ?? parent?.ResolveFunc(name);
    }

    public ObjectAstNode? ResolveObject(string name)
    {
        return _objects.GetValueOrDefault(name) ?? parent?.ResolveObject(name);
    }

    public Type? ResolveType(string name)
    {
        return _types.GetValueOrDefault(name) ?? parent?.ResolveType(name);
    }
}
