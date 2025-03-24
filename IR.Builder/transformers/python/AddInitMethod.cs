using me.vldf.jsa.dsl.ir.context;
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

        var selfType = new TypeReference(node.Name, node.Context);
        var selfArg = new FunctionArgAstNode("self", selfType, 0);
        var initFuncBodyStatements = new List<IStatementAstNode>();
        var initFuncBody = new StatementsBlockAstNode(initFuncBodyStatements);
        var initFuncContext = new IrContext(node.Context, node.Context.Package);
        var initFunc = new FunctionAstNode("__init__", [selfArg], selfType, initFuncBody, node, initFuncContext);

        node.Children = node.Children.Prepend(initFunc).ToList();

        foreach (var fieldDecl in fields)
        {
            var initValue = fieldDecl.Init;
            if (initValue == null)
            {
                continue;
            }

            var fieldRef = new VariableReference(fieldDecl.Name, node.Context);
            var fieldAssignStatement = new AssignmentAstNode(new VarExpressionAstNode(fieldRef), initValue);
            initFuncBodyStatements.Add(fieldAssignStatement);
        }

        var selfArgRef = new VariableReference(selfArg.Name, initFuncContext);
        var returnSelfStatement = new ReturnStatementAstNode(new VarExpressionAstNode(selfArgRef));
        initFuncBodyStatements.Add(returnSelfStatement);

        return node;
    }
}
