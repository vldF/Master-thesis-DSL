using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.builder.checkers.python;

public class InitMethodChecker(ErrorManager errorManager) : AbstractChecker<Unit>
{
    protected override Unit? Merge(Unit? first, Unit? second)
    {
        return first ?? second;
    }

    protected override Unit CheckFunction(FunctionAstNode functionAstNode)
    {
        base.CheckFunction(functionAstNode);

        if (functionAstNode.Name != "__init__" || functionAstNode.Parent == null)
        {
            return new Unit();
        }

        var parent = (ObjectAstNode)functionAstNode.Parent!;

        if (functionAstNode.Body.Children.Any(c => c is ReturnStatementAstNode))
        {
            errorManager.Report(Error.InitFuncCanNotHaveReturn(parent.Name));
            return new Unit();
        }

        if (functionAstNode.ReturnTypeRef != null
            && functionAstNode.ReturnTypeRef.Name != parent.Name
            && functionAstNode.ReturnTypeRef.Name != "any") // todo
        {
            errorManager.Report(Error.InitFuncCanReturnOnlyObjectType(parent.Name));
            return new Unit();
        }

        return new Unit();
    }

    protected override Unit CheckQualifiedAccessBase(QualifiedAccessAstNodeBase qualifiedAccessAstNodeBase)
    {
        return new Unit();
    }

    protected override Unit CheckFunctionCall(FunctionCallAstNode functionCallAstNode)
    {
        return new Unit();
    }

    protected override Unit CheckVarDecl(VarDeclAstNode varDeclAstNode)
    {
        return new Unit();
    }

    protected override Unit CheckIntrinsicFunction(IntrinsicFunctionAstNode intrinsicFunctionAstNode)
    {
        return new Unit();
    }

    protected override Unit CheckFunctionArg(FunctionArgAstNode functionArgAstNode)
    {
        return new Unit();
    }
}
