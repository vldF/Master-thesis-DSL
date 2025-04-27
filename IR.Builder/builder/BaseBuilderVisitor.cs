using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.builder.exceptions;
using me.vldf.jsa.dsl.ir.builder.utils;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.references;
using me.vldf.jsa.dsl.ir.types;
using me.vldf.jsa.dsl.parser;

namespace me.vldf.jsa.dsl.ir.builder.builder;

public class BaseBuilderVisitor(IrContext irContext) : JSADSLBaseVisitor<IAstNode>
{
    private readonly ExpressionBuilderVisitor _expessionBuilderVisitor = new(irContext);

    public override FileAstNode VisitFile(JSADSLParser.FileContext context)
    {
        var package = context.packageDecl()?.package?.Text.Trim('"');
        var result = context
            .topLevelDecl()
            .Select(Visit)
            .Where(x => x is IStatementAstNode)
            .Cast<IStatementAstNode>()
            .ToList();

        var fileAstNode = new FileAstNode(
            package,
            result,
            irContext);

        foreach (var statementAstNode in result)
        {
            if (statementAstNode is VarDeclAstNode varNode)
            {
                varNode.Parent = fileAstNode;
            }
        }

        return fileAstNode;
    }

    public override FunctionAstNode VisitFuncDecl(JSADSLParser.FuncDeclContext context)
    {
        var name = context.name.Text;

        var newContext = new IrContext(irContext);
        var newVisitor = new BaseBuilderVisitor(newContext);

        var args = new List<FunctionArgAstNode>();
        var argIndex = 0;
        foreach (var argContext in context.functionArgs().functionArg())
        {
            var argName = argContext.name.Text;
            var argTypeName = argContext.type?.Text;
            var argType = argTypeName == null ? irContext.AnyTypeRef : new TypeReference(argTypeName, newContext);
            if (argType == null)
            {
                throw new UnresolvedTypeException(argTypeName!);
            }

            IExpressionAstNode? defaultVal = null;
            if (argContext.default_val != null)
            {
                defaultVal = VisitExpression(argContext.default_val);
            }

            var arg = new FunctionArgAstNode(argName, argType, argIndex++, defaultVal);

            args.Add(arg);
            newContext.SaveNewArg(arg);
        }

        var resultTypeName = context.resultType?.Text;
        var resultTypeRef = resultTypeName != null
            ? new TypeReference(resultTypeName, newContext)
            : irContext.AnyTypeRef;

        if (resultTypeName != null && resultTypeRef == null)
        {
            throw new UnresolvedTypeException(resultTypeName);
        }

        var statementsBlockAstNode = newVisitor.VisitStatementsBlock(context.statementsBlock());
        var annotations = context.annotation().Select(VisitAnnotation).ToList();
        var functionAstNode = new FunctionAstNode(
            name,
            args,
            resultTypeRef,
            statementsBlockAstNode,
            (ObjectAstNode?)irContext.AstNode,
            annotations,
            newContext);
        irContext.SaveNewFunc(functionAstNode);
        statementsBlockAstNode.Parent = functionAstNode;

        return functionAstNode;
    }

    public override IntrinsicFunctionAstNode VisitIntrinsicFuncDecl(JSADSLParser.IntrinsicFuncDeclContext context)
    {
        var name = context.name.Text;

        var args = new List<FunctionArgAstNode>();
        var argIndex = 0;
        foreach (var argContext in context.functionArgs().functionArg())
        {
            var argName = argContext.name.Text;
            var argTypeName = argContext.type?.Text;
            var argType = argTypeName == null ? irContext.AnyTypeRef : new TypeReference(argTypeName, irContext);
            var arg = new FunctionArgAstNode(argName, argType, argIndex++);

            args.Add(arg);
        }

        var resultTypeName = context.resultType?.Text;
        var resultTypeRef = resultTypeName == null
            ? irContext.AnyTypeRef
            : new TypeReference(resultTypeName, irContext);

        if (resultTypeName != null && resultTypeRef == null)
        {
            throw new UnresolvedTypeException(resultTypeName);
        }

        var generics = context
            .generic()
            ?.ID()
            ?.Select(id => new GenericType(id.GetText()))
            .ToArray() ?? [];

        foreach (var genericType in generics)
        {
            irContext.SaveNewType(genericType);
        }

        var functionAstNode = new IntrinsicFunctionAstNode(
            name,
            args,
            generics,
            resultTypeRef,
            (ObjectAstNode?)irContext.AstNode);

        irContext.SaveNewFunc(functionAstNode);

        return functionAstNode;
    }

    public override IAstNode VisitObjectDecl(JSADSLParser.ObjectDeclContext context)
    {
        var name = context.name.Text;

        var newContext = new IrContext(irContext);
        var newVisitor = new BaseBuilderVisitor(newContext);

        var children = new List<IAstNode>();
        var annotations = context.annotation().Select(VisitAnnotation).ToList();
        var result = new ObjectAstNode(name, children, annotations, newContext);
        newContext.AstNode = result;

        var newObjectType = new TypeReference(name, irContext);

        var selfFakeVarDecl = new VarDeclAstNode("self", newObjectType, null)
        {
            IsSelf = true
        };
        newContext.SaveNewVar(selfFakeVarDecl);

        var astNodes = context.objectBody().children?.SelectNotNull(c => newVisitor.Visit(c)).ToList();
        foreach (var astNode in astNodes ?? [])
        {
            if (astNode is IStatementAstNode statementAstNode)
            {
                statementAstNode.Parent = result;
            }
        }
        children.AddRange(astNodes ?? []);

        var type = new ObjectAstType(result);
        irContext.SaveNewType(type);
        irContext.SaveNewObject(result);

        return result;
    }

    public override IStatementAstNode VisitIfStatement(JSADSLParser.IfStatementContext context)
    {
        var cond = VisitExpression(context.cond);
        var mainBlockContext = new IrContext(irContext);
        var mainBlockVisitor = new BaseBuilderVisitor(mainBlockContext);

        var mainBlock = mainBlockVisitor.VisitStatementsBlock(context.mainBlock);
        IStatementAstNode? elseBranch = null;

        if (context.elseIfStatement != null)
        {
            var newContext = new IrContext(irContext);
            var newVisitor = new BaseBuilderVisitor(newContext);

            elseBranch = newVisitor.VisitIfStatement(context.elseIfStatement);
        } else if (context.elseBlock != null)
        {
            var newContext = new IrContext(irContext);
            var newVisitor = new BaseBuilderVisitor(newContext);

            elseBranch = newVisitor.VisitStatementsBlock(context.elseBlock);
        }

        var ifStatementAstNode = new IfStatementAstNode(cond, mainBlock, elseBranch);

        mainBlock.Parent = ifStatementAstNode;
        if (elseBranch != null)
        {
            elseBranch.Parent = ifStatementAstNode;
        }

        return ifStatementAstNode;
    }

    public override StatementsBlockAstNode VisitStatementsBlock(JSADSLParser.StatementsBlockContext context)
    {
        var children = context.statement().SelectNotNull(VisitStatement).ToList();

        var statementsBlockAstNode = new StatementsBlockAstNode(children);
        foreach (var statementAstNode in children)
        {
            statementAstNode.Parent = statementsBlockAstNode;
        }

        statementsBlockAstNode.Context = irContext;

        return statementsBlockAstNode;
    }

    public override IStatementAstNode VisitAssignmentStatement(JSADSLParser.AssignmentStatementContext context)
    {
        var assignee = _expessionBuilderVisitor.VisitExpression(context.assignee);
        var value = _expessionBuilderVisitor.VisitExpression(context.value);

        return new AssignmentAstNode(assignee, value);
    }

    public override IStatementAstNode VisitVarDeclarationStatement(JSADSLParser.VarDeclarationStatementContext context)
    {
        var varName = context.varName.Text;
        var initValue = context.initValue;
        var typeName = context.type?.Text;

        var value = initValue != null ? _expessionBuilderVisitor.VisitExpression(initValue) : null;
        var type = typeName != null ? new TypeReference(typeName, irContext) : null;

        var varDeclarationStatement = new VarDeclAstNode(varName, type, value);
        irContext.SaveNewVar(varDeclarationStatement);

        return varDeclarationStatement;
    }

    public override IStatementAstNode VisitReturnStatement(JSADSLParser.ReturnStatementContext context)
    {
        var expressionContext = context.expression();
        if (expressionContext == null)
        {
            return new ReturnStatementAstNode(null);
        }

        var expression = VisitExpression(expressionContext);
        return new ReturnStatementAstNode(expression);
    }


    public override IExpressionAstNode VisitExpression(JSADSLParser.ExpressionContext context)
    {
        return _expessionBuilderVisitor.VisitExpression(context);
    }

    public override FunctionCallAstNode VisitFunctionCall(JSADSLParser.FunctionCallContext context)
    {
        return _expessionBuilderVisitor.VisitFunctionCall(context);
    }

    public override IAstNode VisitImportDecl(JSADSLParser.ImportDeclContext context)
    {
        var package = context.package.Text.Trim('"');
        irContext.SaveImport(package);

        return new ImportAstNode(package);
    }

    public override AnnotationAstNode VisitAnnotation(JSADSLParser.AnnotationContext context)
    {
        return new AnnotationAstNode(
            context.ID().GetText(),
            context.expressionList().expression().Select(VisitExpression).ToList());
    }

    public override IStatementAstNode VisitStatement(JSADSLParser.StatementContext context)
    {
        if (context.ifStatement() != null)
        {
            return VisitIfStatement(context.ifStatement());
        }

        if (context.assignmentStatement() != null)
        {
            return VisitAssignmentStatement(context.assignmentStatement());
        }

        if (context.varDeclarationStatement() != null)
        {
            return VisitVarDeclarationStatement(context.varDeclarationStatement());
        }

        if (context.returnStatement() != null)
        {
            return VisitReturnStatement(context.returnStatement());
        }

        if (context.functionCall() != null)
        {
            return VisitFunctionCall(context.functionCall());
        }

        throw new ArgumentOutOfRangeException(nameof(context));
    }

    public override IAstNode VisitObjectBodyStatement(JSADSLParser.ObjectBodyStatementContext context)
    {
        if (context.varDeclarationStatement() != null)
        {
            return VisitVarDeclarationStatement(context.varDeclarationStatement());
        }

        if (context.funcDecl() != null)
        {
            return VisitFuncDecl(context.funcDecl());
        }

        throw new ArgumentOutOfRangeException(nameof(context));
    }

    public override IAstNode VisitTopLevelDecl(JSADSLParser.TopLevelDeclContext context)
    {
        if (context.varDeclarationStatement() != null)
        {
            return VisitVarDeclarationStatement(context.varDeclarationStatement());
        }

        if (context.objectDecl() != null)
        {
            return VisitObjectDecl(context.objectDecl());
        }

        if (context.funcDecl() != null)
        {
            return VisitFuncDecl(context.funcDecl());
        }

        if (context.intrinsicFuncDecl() != null)
        {
            return VisitIntrinsicFuncDecl(context.intrinsicFuncDecl());
        }

        if (context.importDecl() != null)
        {
            return VisitImportDecl(context.importDecl());
        }

        throw new Exception("unknown");
    }
}
