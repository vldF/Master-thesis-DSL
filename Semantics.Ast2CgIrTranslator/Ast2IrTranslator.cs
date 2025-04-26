using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.statements;
using me.vldf.jsa.dsl.ast.visitors;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;
using Semantics.Ast2CgIrTranslator.Emitters;

namespace Semantics.Ast2CgIrTranslator;

public class Ast2IrTranslator : IAstVisitor
{
    private readonly TranslatorContext _ctx = new();
    private readonly MethodEmitter _methodEmitter;

    public Ast2IrTranslator()
    {
        _methodEmitter = new MethodEmitter(_ctx);
    }

    public CgFile Translate(FileAstNode node)
    {
        VisitFileAstNode(node);
        return _ctx.File;
    }

    // todo: extract a dedicated phase
    public void VisitFileAstNode(FileAstNode node)
    {
        var fileName = Path.GetFileNameWithoutExtension(node.FileName!) + ".jsa";
        _ctx.File = CreateFile(fileName);
        _ctx.PushContainer(_ctx.File);

        var objectDeclarations = node.TopLevelDeclarations
            .Where(decl => decl is ObjectAstNode)
            .ToList();
        foreach (var decl in objectDeclarations)
        {
            var objectDecl = decl as ObjectAstNode;

            _ctx.ClassDescriptorVariables[objectDecl!.Name] = new CgVarExpression(objectDecl.GetDescriptionVarName());
        }

        foreach (var nodeTopLevelDeclaration in node.TopLevelDeclarations)
        {
            ((IAstVisitor)this).VisitStatementAstNode(nodeTopLevelDeclaration);
        }
        _ctx.PopContainer();
    }

    public void VisitFunctionArgAstNode(FunctionArgAstNode node) {}

    public void VisitFunctionAstNode(FunctionAstNode node)
    {
        _methodEmitter.Emit(node);
    }

    public void VisitObjectAstNode(ObjectAstNode node)
    {
        var classDescriptorVariableExpr = _ctx.Semantics.SemanticsApi.CallMethod(
            "CreateClassObjectDescriptor",
            [AsExpression(node.Name)]);

        var classDescriptorVariable = _ctx.File.AddVarDecl(
            node.GetDescriptionVarName(),
            init: classDescriptorVariableExpr).AsValue();

        _ctx.ClassDescriptorVariables[node.Name] = classDescriptorVariable;
        _ctx.CurrentClassDescriptor = classDescriptorVariable;

        foreach (var child in node.Children)
        {
            ((IAstVisitor)this).Visit(child);
        }

        var moduleDescr = _ctx.Semantics.ModuleDescriptorVar;
        var moduleAssignStatement = _ctx.Semantics.InterpreterApi.CallMethod(
            "Assign",
            [moduleDescr, AsExpression(node.Name), classDescriptorVariable]);
        _ctx.File.Statements.Add(moduleAssignStatement);

        var pythonBuildClassStatement = _ctx.Semantics.Types.CallMethod(
            "BuildClass",
            [classDescriptorVariable]);
        _ctx.File.Statements.Add(pythonBuildClassStatement);
    }

    public void VisitImportAstNode(ImportAstNode importAstNode)
    {
        _ctx.File.Statements.Add(new CgDirectiveStatement("load", importAstNode.FileName + ".jsa"));
    }

    public void VisitVarDeclAstNode(VarDeclAstNode node)
    {
        if (node.Parent is not FileAstNode)
        {
            return;
        }

        if (node.Init != null)
        {
            var expressionsEmitter = new ExpressionsEmitter(_ctx);
            var initValue = expressionsEmitter.EmitExpression(node.Init);

            var var = VarDeclaration(
                _ctx.File,
                node.Name,
                type: null,
                init: initValue);

            var assignStatement = _ctx.Semantics.InterpreterApi.CallMethod("Assign", [
                _ctx.Semantics.ModuleDescriptorVar,
                AsExpression(node.Name),
                var]);

            _ctx.File.Statements.Add(assignStatement);
        }
        else
        {
            throw new Exception("top level variable must have initialization expression");
        }
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

    public void VisitVarAssignmentAstNode(AssignmentAstNode node)
    {
    }

    public void VisitIntrinsicFunctionAstNode(IntrinsicFunctionAstNode node)
    {
    }
}
