using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.builder.utils;

public static class AstHelpers
{
    public static VarExpressionAstNode GetFakeVariable(
        this IrContext ctx,
        string name,
        bool isOutVar = false,
        IExpressionAstNode? initialValue = null)
    {
        var interpretatorFakeDecl = new VarDeclAstNode(name, null, initialValue);
        ctx.SaveNewVar(interpretatorFakeDecl);
        return new VarExpressionAstNode(
            new VariableReference(name, ctx),
            isOutVar: isOutVar);
    }

    public static VariableReference GetVarRef(this VarDeclAstNode varDecl, IrContext irContext)
    {
        return new VariableReference(varDecl.Name, irContext);
    }

    public static VarExpressionAstNode GetVarExpr(this VarDeclAstNode varDecl, IrContext irContext)
    {
        return new VarExpressionAstNode(varDecl.GetVarRef(irContext));
    }

    public static ObjectAstNode? ResolveObjectByType(this IrContext context, ObjectAstType obj)
    {
        var objReference = new ObjectReference(obj.Name, context);
        return objReference.Resolve();
    }
}
