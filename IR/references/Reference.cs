using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using TypeReference = me.vldf.jsa.dsl.ir.references.Reference<me.vldf.jsa.dsl.ast.types.AstType>;
using FunctionReference = me.vldf.jsa.dsl.ir.references.Reference<me.vldf.jsa.dsl.ir.nodes.declarations.FunctionAstNode>;
using ObjectReference = me.vldf.jsa.dsl.ir.references.Reference<me.vldf.jsa.dsl.ir.nodes.declarations.ObjectAstNode>;
using VariableReference = me.vldf.jsa.dsl.ir.references.Reference<me.vldf.jsa.dsl.ir.nodes.declarations.VarDeclAstNode>;

namespace me.vldf.jsa.dsl.ir.references;

public class Reference<T>(string id)
{
    public string Id { get; } = id;
    public T? SealedValue { get; set; } = default;
}

public class TypeReference(string id) : Reference<AstType>(id);

public class FunctionReference(string id) : Reference<FunctionAstNode>(id);

public class ObjectReference(string id) : Reference<ObjectAstNode>(id);

public class VariableReference(string id) : Reference<VarDeclAstNode>(id);
