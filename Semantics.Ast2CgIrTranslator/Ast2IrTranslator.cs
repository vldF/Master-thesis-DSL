using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.types;
using me.vldf.jsa.dsl.ast.nodes.declarations;
using me.vldf.jsa.dsl.ast.nodes.expressions;
using me.vldf.jsa.dsl.ast.nodes.statements;
using me.vldf.jsa.dsl.ast.visitors;
using static Codegen.IR.Builder.CodegenIrBuilder;

namespace Semantics.Ast2CgIrTranslator;

public class Ast2IrTranslator : IAstVisitor
{
#pragma warning disable CS8618
    private CgFile _file;
    private ICgExpression _currentBuilder;
    private CgMethod _handlerMethod;
#pragma warning restore CS8618

    public CgFile Translate(FileAstNode node)
    {
        VisitFileAstNode(node);
        return _file;
    }

    public void VisitFileAstNode(FileAstNode node)
    {
        _file = CreateFile("file.jsadsl");

        foreach (var nodeTopLevelDeclaration in node.TopLevelDeclarations)
        {
            ((IAstVisitor)this).VisitStatementAstNode(nodeTopLevelDeclaration);
        }
    }

    public void VisitFunctionArgAstNode(FunctionArgAstNode node)
    {
    }

    public void VisitFunctionAstNode(FunctionAstNode node)
    {
        _currentBuilder = _currentBuilder.CallMethod(
            "WithMethod",
            [AsExpression(node.Name), new CgVarExpression(node.GetHandlerName())]);

        var locationArgName = "location";
        var locationArgType = new CgSimpleType("Location");

        var functionCallName = "functionCall";
        var functionCallType = new CgSimpleType("FunctionCall");

        var args = new Dictionary<string, ICgType>
        {
            { locationArgName, locationArgType },
            { functionCallName, functionCallType },
        };

        var returnType = new CgSimpleType("CallHandlerResult");

        _handlerMethod = _file.CreateMethod(node.GetHandlerName(), args, returnType);

        foreach (var bodyChild in node.Body.Children)
        {
            ((IAstVisitor)this).Visit(bodyChild);
        }

        _handlerMethod.AddReturn(
            new CgVarExpression("CallHandlerResult")
                .CallMethod("Processed", [new CgVarExpression("SemanticsApi").Property("None")]));
    }

    public void VisitObjectAstNode(ObjectAstNode node)
    {
        var pythonTypes = new CgVarExpression("PythonTypes");
        _currentBuilder = pythonTypes
            .CallMethod("CreateClass", [AsExpression(node.Name)]);

        foreach (var child in node.Children)
        {
            ((IAstVisitor)this).Visit(child);
        }

        _file.AddVarDecl(node.GetDescriptionVarName(), init: _currentBuilder);
    }

    public void VisitVarDeclAstNode(VarDeclAstNode node)
    {
    }

    public void VisitExpressionAstNode(IExpressionAstNode node)
    {
    }

    public void VisitVarExpressionAstNode(VarExpressionAstNode node)
    {
    }

    public void VisitIfStatementAstNode(IfStatementAstNode node)
    {
    }

    public void VisitReturnStatementAstNode(ReturnStatementAstNode node)
    {
    }

    public void VisitStatementsBlockAstNode(StatementsBlockAstNode node)
    {
    }

    public void VisitVarAssignmentAstNode(VarAssignmentAstNode node)
    {
    }

}
