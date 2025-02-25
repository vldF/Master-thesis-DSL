using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
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

        foreach (var decl in objectDeclarations)
        {
            var objectDecl = decl as ObjectAstNode;

            var classDescriptorVariable = _ctx.ClassDescriptorVariables[objectDecl!.Name];

            var interpreterVar = new CgVarExpression("Interpreter");
            var moduleDescriptorVar = new CgVarExpression("ModuleDescriptor");
            var moduleAssignmentStatement = new CgMethodCall(interpreterVar,
                "Assign",
                [
                    moduleDescriptorVar,
                    new CgStringLiteral(objectDecl.Name),
                    classDescriptorVariable.Property("ClassDescriptor")
                ]);

            _ctx.File.Statements.Add(moduleAssignmentStatement);
        }
    }

    public void VisitFunctionArgAstNode(FunctionArgAstNode node) {}

    public void VisitFunctionAstNode(FunctionAstNode node)
    {
        _methodEmitter.Emit(node);
    }

    public void VisitObjectAstNode(ObjectAstNode node)
    {
        _ctx.CurrentBuilder = _ctx.Semantics.CreateClass(node.Name);

        foreach (var child in node.Children)
        {
            ((IAstVisitor)this).Visit(child);
        }

        var classDescriptorVariable = _ctx.File.AddVarDecl(
            node.GetDescriptionVarName(),
            init: _ctx.CurrentBuilder);

        _ctx.ClassDescriptorVariables[node.Name] = classDescriptorVariable.AsValue();
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

    public void VisitIntrinsicFunctionAstNode(IntrinsicFunctionAstNode node)
    {
    }
}
