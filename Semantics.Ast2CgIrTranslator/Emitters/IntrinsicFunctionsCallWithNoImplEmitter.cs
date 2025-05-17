using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.statements;
using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace Semantics.Ast2CgIrTranslator.Emitters;

public class IntrinsicFunctionsCallWithNoImplEmitter(TranslatorContext ctx, ExpressionsEmitter expressionsEmitter)
{
    private readonly Dictionary<string, IFunctionEmitter> _supportedIntrinsicFunctions = new()
    {
        { "CreateTaintedDataOfType", new CreateDataOfTypeEmitter(isTainted: true, ctx, expressionsEmitter) },
        { "CreateDataOfType", new CreateDataOfTypeEmitter(isTainted: false, ctx, expressionsEmitter) },
    };

    public bool IsApplicable(IntrinsicFunctionInvocationAstNode call)
    {
        return call.Reciever == null && _supportedIntrinsicFunctions.ContainsKey(call.Name);
    }

    public ICgExpression Emit(IntrinsicFunctionInvocationAstNode call)
    {
        return _supportedIntrinsicFunctions[call.Name].Emit(call);
    }

    interface IFunctionEmitter
    {
        public ICgExpression Emit(IntrinsicFunctionInvocationAstNode call);
    }

    class CreateDataOfTypeEmitter(bool isTainted, TranslatorContext ctx, ExpressionsEmitter expressionsEmitter) : IFunctionEmitter
    {
        private int _idCounter = 0;

        private static Dictionary<string, string> _buildInTypesMappint = new()
        {
            { "string", "StringType" },
            { "int", "LongType" },
            { "float", "FloatType" },
            { "bool", "BoolType" },
            { "bytes", "BytesType" },
            { "any", "ObjectType" },
            { "dict", "DictType" },
            { "list", "ListType" },
        };

        public ICgExpression Emit(IntrinsicFunctionInvocationAstNode call)
        {
            var type = call.Generics.First().Resolve()!;
            var result = ctx.Semantics.CreateNonTypedInstance(
                $"<dsl_{(isTainted ? "tainted" : "" )}_data_{_idCounter++}>",
                []);
            if (type is SimpleAstType)
            {
                var typePropName = _buildInTypesMappint[type.Name];
                if (typePropName == null)
                {
                    throw new InvalidOperationException($"unsupported simple type {type.Name}");
                }

                var typeProviderCgExpr = ctx.Semantics.SemanticsApi.Property(typePropName);
                result = result.CallMethod("WithType", [typeProviderCgExpr]);
            }

            if (isTainted)
            {
                var taintOrigin = call.Args.ToList()[1];
                if (taintOrigin is not StringLiteralAstNode && taintOrigin is not IntrinsicFunctionInvocationAstNode)
                {
                    throw new InvalidOperationException("taint origin must be a constant string or the GetTaintOrigin function call");
                }

                ICgExpression taintOriginCgExpr;
                if (taintOrigin is IntrinsicFunctionInvocationAstNode getTaintOriginCall)
                {
                    if (getTaintOriginCall.Name != "GetTaintOrigin")
                    {
                        throw new InvalidOperationException("only GetTaintOrigin call allowed");
                    }

                    taintOriginCgExpr = expressionsEmitter.EmitExpression(getTaintOriginCall);
                }
                else
                {
                    var taintOriginConstStr = (StringLiteralAstNode)taintOrigin;
                    taintOriginCgExpr = AsExpression(taintOriginConstStr.Value);
                }

                taintOriginCgExpr = new CgNewExpression("TaintOrigin", [taintOriginCgExpr]);
                result = result.CallMethod("With", [taintOriginCgExpr]);
            }

            var resultVarName = ctx.GetFreshVarName("teintData");
            var resultTempVarDecl = new CgVarDeclStatement(resultVarName, null, result);
            ctx.CurrentContainer!.Add(resultTempVarDecl);

            return new CgVarExpression(resultVarName);
        }
    }
}
