using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.builder.transformers.python;

public class AddInitMethod : AbstractAstSemanticTransformer
{
    protected override ObjectAstNode TransformObjectAstNode(ObjectAstNode node)
    {
        var fields = node.Children.OfType<VarDeclAstNode>().ToList();

        if (fields.Count == 0)
        {
            return node;
        }

        GetOrCreateInitMethod(node,
            out var initFuncContext,
            out var initFunc);

        var newStatements = new List<IAstNode>();

        foreach (var fieldDecl in fields)
        {
            var initValue = fieldDecl.Init;
            if (initValue == null)
            {
                continue;
            }

            var fieldRef = new VariableReference(fieldDecl.Name, node.Context);
            var fieldAssignStatement = new AssignmentAstNode(new VarExpressionAstNode(fieldRef), initValue);
            newStatements.Add(fieldAssignStatement);
        }

        var existingBodyStatements = initFunc.Body.Children.ToList();
        existingBodyStatements.InsertRange(0, newStatements);

        var selfArgRef = new VariableReference("self", initFuncContext);
        var returnSelfStatement = new ReturnStatementAstNode(new VarExpressionAstNode(selfArgRef));
        existingBodyStatements.Add(returnSelfStatement);

        initFunc.Body.Children = existingBodyStatements;

        return node;
    }

    private static void GetOrCreateInitMethod(
        ObjectAstNode node,
        out IrContext initFuncContext,
        out FunctionAstNode initFunc)
    {
        if (node.Children.OfType<FunctionAstNode>().Any(f => f.Name == "__init__"))
        {
            initFunc = node.Children.OfType<FunctionAstNode>().Single();
            initFunc.ReturnTypeRef = new TypeReference(node.Name, node.Context);
            initFuncContext = node.Context;
        }
        else
        {
            var selfType = new TypeReference(node.Name, node.Context);
            var selfArg = new FunctionArgAstNode("self", selfType, 0);
            var initFuncBody = new StatementsBlockAstNode([]);
            initFuncContext = new IrContext(node.Context, node.Context.Package);
            initFunc = new FunctionAstNode("__init__", [selfArg], selfType, initFuncBody, node, initFuncContext);

            node.Children = node.Children.Prepend(initFunc).ToList();
        }
    }
}
