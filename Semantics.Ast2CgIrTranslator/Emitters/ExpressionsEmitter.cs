using Codegen.IR.nodes.expressions;
using me.vldf.jsa.dsl.ast.nodes.expressions;

namespace Semantics.Ast2CgIrTranslator.Emitters;

public class ExpressionsEmitter(TranslatorContext ctx)
{
    public ICgExpression EmitExpression(IExpressionAstNode expression)
    {
        return expression switch
        {
            NewAstNode newAstNode => EmitNewNode(newAstNode),
            VarExpressionAstNode varExpressionAstNode => new CgVarExpression(varExpressionAstNode.Variable.Name),
            _ => throw new ArgumentOutOfRangeException(nameof(expression))
        };
    }

    private ICgExpression EmitNewNode(NewAstNode newAstNode)
    {
        var descriptor = ctx.ClassDescriptorVariables[newAstNode.objectName];
        var args = newAstNode.args.Select(EmitExpression);
        return ctx.Semantics.CreateInstance(newAstNode.objectName, descriptor, args);
    }
}
