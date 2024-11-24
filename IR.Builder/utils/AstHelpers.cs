using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.builder.utils;

public static class AstHelpers
{
    public static VarExpressionAstNode GetFakeVariable(this IrContext ctx, string name)
    {
        var interpretatorFakeDecl = new VarDeclAstNode(name, null, null);
        ctx.SaveNewVar(interpretatorFakeDecl);
        return new VarExpressionAstNode(
            new VariableReference(name, ctx));
    }
}
