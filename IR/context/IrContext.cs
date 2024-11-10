using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.context;

public class IrContext(IrContext? parent)
{
    private readonly Dictionary<string, VarDeclAstNode> _vars = new ();
    private readonly Dictionary<string, FunctionAstNode> _funcs = new ();
    private readonly Dictionary<string, ObjectAstNode> _objects = new ();
    private readonly Dictionary<string, AstType> _types = new();

    public readonly TypeReference AnyTypeRef = new("any");

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


    public VarDeclAstNode? ResolveVar(VariableReference reference)
    {
        return _vars.GetValueOrDefault(reference.Id) ?? parent?.ResolveVar(reference);
    }

    public FunctionAstNode? ResolveFunc(FunctionReference reference)
    {
        return _funcs.GetValueOrDefault(reference.Id) ?? parent?.ResolveFunc(reference);
    }

    public ObjectAstNode? ResolveObject(ObjectReference reference)
    {
        return _objects.GetValueOrDefault(reference.Id) ?? parent?.ResolveObject(reference);
    }

    public AstType? ResolveType(TypeReference reference)
    {
        return _types.GetValueOrDefault(reference.Id) ?? parent?.ResolveType(reference);
    }
}
