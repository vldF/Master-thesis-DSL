using me.vldf.jsa.dsl.ir.helpers;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class SemanticBinaryOperationsTransformer : AbstractAstSemanticTransformer
{
    protected override IExpressionAstNode TransforBinaryAstNode(BinaryExpressionAstNode node)
    {
        node = (BinaryExpressionAstNode)base.TransforBinaryAstNode(node);

        var semanticEntityName = node.Op switch
        {
            BinaryOperation.Mul => "MultiplyDescriptor",
            BinaryOperation.Div => "DivDescriptor",
            BinaryOperation.Mod => "RemDescriptor",
            BinaryOperation.Sum => "AddDescriptor",
            BinaryOperation.Sub => "SubDescriptor",
            BinaryOperation.Eq => "EqDescriptor",
            BinaryOperation.NotEq => "NotEqDescriptor",
            BinaryOperation.LtEq => "LessOrEqDescriptor",
            BinaryOperation.Lt => "LessDescriptor",
            BinaryOperation.GtEq => "GreaterOrEqDescriptor",
            BinaryOperation.Gt => "GreaterDescriptor",
            BinaryOperation.AndAnd => "AndBitwiseDescriptor",
            BinaryOperation.OrOr => "OrBitwiseDescriptor",
            BinaryOperation.Xor => "XorDescriptor",
            _ => throw new ArgumentOutOfRangeException()
        };

        var semanticEntityAccess = SemanticsApi.Property(semanticEntityName);

        return new IntrinsicFunctionInvokationAstNode(
            Interpretor,
            "InvokeFunction",
            [LocationArg, semanticEntityAccess, node.Left, node.Right],
            []);
    }
}
