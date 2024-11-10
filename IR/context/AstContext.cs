using me.vldf.jsa.dsl.ast.nodes.declarations;
using me.vldf.jsa.dsl.ast.types;

namespace me.vldf.jsa.dsl.ast.context;

public class AstContext(AstContext? parent)
{
    private readonly Dictionary<string, VarDeclAstNode> _vars = new ();
    private readonly Dictionary<string, FunctionAstNode> _funcs = new ();
    private readonly Dictionary<string, ObjectAstNode> _objects = new ();
    private readonly Dictionary<string, AstType> _types = new();

    public readonly AstType AnyType = new SimpleAstType("any");

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

    public void SaveNewType(AstType astType)
    {
        _types[astType.Name] = astType;
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

    public AstType? ResolveType(string name)
    {
        return _types.GetValueOrDefault(name) ?? parent?.ResolveType(name);
    }
}
