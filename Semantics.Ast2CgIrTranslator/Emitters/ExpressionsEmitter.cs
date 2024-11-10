using Codegen.IR.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace Semantics.Ast2CgIrTranslator.Emitters;

public class ExpressionsEmitter(TranslatorContext ctx)
{
    public ICgExpression EmitExpression(IExpressionAstNode expression)
    {
        return expression switch
        {
            NewAstNode newAstNode => EmitNewNode(newAstNode),
            VarExpressionAstNode varExpressionAstNode => new CgVarExpression(varExpressionAstNode.VariableReference.Resolve()!.Name),
            _ => throw new ArgumentOutOfRangeException(nameof(expression))
        };
    }

    private ICgExpression EmitNewNode(NewAstNode newAstNode)
    {
        var obj = newAstNode.TypeReference.SealedValue!;
        var descriptor = ctx.ClassDescriptorVariables[obj.Name];
        var args = newAstNode.Args.Select(EmitExpression);
        return ctx.Semantics.CreateInstance(obj.Name, descriptor, args);
    }
}
