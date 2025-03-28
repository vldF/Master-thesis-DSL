using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.builder.transformers.utils;
using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.types;

namespace me.vldf.jsa.dsl.ir.builder.checkers;

public class TypeChecker(ErrorManager errorManager) : AbstractChecker<AstType>
{
    private FunctionAstNodeBase? _currentFunc;

    private HashSet<AstType> _numericTypes = [SimpleAstType.Float, SimpleAstType.Int, SimpleAstType.Any];

    protected override AstType? Merge(AstType? first, AstType? second)
    {
        if (first == null)
        {
            return second;
        }

        if (second == null)
        {
            return null;
        }

        if (first == second)
        {
            return first;
        }

        errorManager.Report(Error.TypeMissmatch(first, second));
        return null;

    }

    protected override AstType? CheckAssignment(AssignmentAstNode assignmentAstNode)
    {
        var expectedType = CheckExpression(assignmentAstNode.Reciever);
        if (expectedType == null)
        {
            return null;
        }

        var actualType = CheckExpression(assignmentAstNode.Value);
        if (actualType == null)
        {
            return null;
        }

        if (!CheckTypes(expectedType, actualType))
        {
            errorManager.Report(Error.TypeMissmatch(expectedType, actualType));
            return null;
        }

        return null;
    }

    protected override AstType? CheckIf(IfStatementAstNode ifStatementAstNode)
    {
        var boolType = SimpleAstType.Bool;
        var exprType = CheckExpression(ifStatementAstNode.Cond);

        if (exprType == null)
        {
            return null;
        }

        if (!CheckTypes(boolType, exprType))
        {
            errorManager.Report(Error.TypeMissmatch(boolType, exprType));
            return null;
        }

        return base.CheckIf(ifStatementAstNode);
    }

    protected override AstType? CheckFunction(FunctionAstNode functionAstNode)
    {
        _currentFunc = functionAstNode;
        base.CheckFunction(functionAstNode);
        _currentFunc = null;
        return null;
    }

    protected override AstType? CheckReturn(ReturnStatementAstNode returnStatementAstNode)
    {
        if (base.CheckReturn(returnStatementAstNode) == null)
        {
            return null;
        }

        var expr = returnStatementAstNode.Expression;

        var expectedTypeRef = _currentFunc!.ReturnTypeRef;
        if (expectedTypeRef == null && expr != null)
        {
            errorManager.Report(Error.UnexpectedReturnExpression(expr));
            return null;
        }

        if (expectedTypeRef != null && expr == null)
        {
            errorManager.Report(Error.NoReturnExpression());
            return null;
        }

        if (expectedTypeRef == null && expr != null)
        {
            errorManager.Report(Error.UnexpectedReturnExpression(expr));
            return null;
        }

        if (expectedTypeRef != null && expr != null)
        {
            var expectedType = expectedTypeRef.Resolve();
            if (expectedType == null)
            {
                errorManager.Report(Error.UnresolvedType(expectedTypeRef.Name));
                return null;
            }
            var actualType = CheckExpression(expr);
            if (actualType == null)
            {
                return null;
            }

            if (CheckTypes(expectedType, actualType))
            {
                return null;
            }
        }

        return null;
    }

    protected override AstType? CheckQualifiedAccessBase(QualifiedAccessAstNodeBase qualifiedAccessAstNodeBase)
    {
        return null;
    }

    protected override AstType? CheckFunctionCall(FunctionCallAstNode functionCallAstNode)
    {
        var func = functionCallAstNode.FunctionReference.Resolve();
        if (func == null)
        {
            errorManager.Report(Error.UnresolvedFunction(functionCallAstNode.FunctionReference.Name));
            return null;
        }

        for (var i = 0; i < functionCallAstNode.Args.Length; i++)
        {
            var expectedTypeRef = func.Args[i].TypeReference;
            AstType expectedType;
            if (expectedTypeRef == null)
            {
                expectedType = SimpleAstType.Any;
            }
            else
            {
                var resolvedType = expectedTypeRef.Resolve();
                if (resolvedType == null)
                {
                    errorManager.Report(Error.UnresolvedType(expectedTypeRef.Name));
                    return null;
                }

                expectedType = resolvedType;
            }

            var actualExpr = functionCallAstNode.Args[i];
            var actualType = CheckExpression(actualExpr);
            if (actualType == null)
            {
                return null;
            }

            if (!CheckTypes(expectedType, actualType))
            {
                errorManager.Report(Error.TypeMissmatch(expectedType, actualType));
                return null;
            }
        }

        var callGenerics = functionCallAstNode.Generics.ToList();
        var genericInfo = new Dictionary<GenericType, AstType>();

        if (callGenerics.Count != 0)
        {
            if (func is not IntrinsicFunctionAstNode calleeIntrinsicFunc)
            {
                errorManager.Report(Error.UnexpectedGenericsCount(0, callGenerics.Count));
                return null;
            }

            var calleeGenerics = calleeIntrinsicFunc.Generics.ToList();

            if (callGenerics.Count != calleeGenerics.Count)
            {
                errorManager.Report(Error.UnexpectedGenericsCount(calleeGenerics.Count, callGenerics.Count));
                return null;
            }

            for (var i = 0; i < callGenerics.Count; i++)
            {
                var actualGenericSubstitutionRef = callGenerics[i];
                var actualGenericSubstitution = actualGenericSubstitutionRef.Resolve()!;

                var expectedGeneric = calleeGenerics[i];
                if (genericInfo.TryGetValue(expectedGeneric, out var existingSubstitution))
                {
                    if (!CheckTypes(existingSubstitution, actualGenericSubstitution))
                    {
                        errorManager.Report(Error.TypeMissmatch(existingSubstitution, actualGenericSubstitution));
                        return null;
                    }
                }
                else
                {
                    genericInfo[expectedGeneric] = actualGenericSubstitution;
                }
            }
        }

        for (var i = 0; i < callGenerics.Count; i++)
        {
            var actualTypeRef = callGenerics[i];
            var actualType = actualTypeRef.Resolve();

            if (actualType == null)
            {
                errorManager.Report(Error.UnresolvedType(actualTypeRef.Name));
                return null;
            }
        }

        for (var i = 0; i < func.Args.Count; i++)
        {
            var typeReference = func.Args[i].TypeReference;
            if (typeReference == null)
            {
                continue;
            }

            var argType = typeReference.Resolve()!;
            if (argType is not GenericType expectedGenericType)
            {
                continue;
            }

            var actialType = CheckExpression(functionCallAstNode.Args[i])!;
            if (genericInfo.TryGetValue(expectedGenericType, out var existingAssociatedType))
            {
                if (!CheckTypes(existingAssociatedType, actialType))
                {
                    errorManager.Report(Error.TypeMissmatch(existingAssociatedType, actialType));
                    return null;
                }
            }
            else
            {
                genericInfo[expectedGenericType] = actialType;
            }
        }

        var returnTypeRef = func.ReturnTypeRef;
        if (returnTypeRef == null)
        {
            return SimpleAstType.Any;
        }

        var returnType = returnTypeRef.Resolve();
        if (returnType == null)
        {
            errorManager.Report(Error.UnresolvedType(returnTypeRef.Name));
            return null;
        }

        if (returnType is not GenericType returnGenericType)
        {
            return returnType;
        }

        if (!genericInfo.TryGetValue(returnGenericType, out var existingAssociatedReturnType))
        {
            errorManager.Report(Error.CanNotInferReturnType(func.Name));
            return null;
        }


        return existingAssociatedReturnType;
    }

    protected override AstType? CheckVarDecl(VarDeclAstNode varDeclAstNode)
    {
        AstType? valueType = null;
        if (varDeclAstNode.Init != null)
        {
            valueType = CheckExpression(varDeclAstNode.Init);
            if (valueType == null)
            {
                return null;
            }
        }

        var typeRef = varDeclAstNode.TypeReference;

        if (valueType == null && typeRef == null)
        {
            errorManager.Report(Error.CanNotInferVarType(varDeclAstNode.Name));
            return null;
        }

        if (typeRef == null)
        {
            var contextOrNull = varDeclAstNode.GetNearestContext();
            if (contextOrNull == null)
            {
                errorManager.Report(Error.CanNotInferVarType(varDeclAstNode.Name));
                return null;
            }

            varDeclAstNode.TypeReference = valueType!.GetReference(contextOrNull);
            return null;
        }

        var type = typeRef.Resolve();
        if (type == null)
        {
            errorManager.Report(Error.UnresolvedType(typeRef.Name));
            return null;
        }

        if (valueType != null && !CheckTypes(type, valueType))
        {
            errorManager.Report(Error.TypeMissmatch(type, valueType));
            return null;
        }

        return null;
    }

    protected override AstType? CheckIntrinsicFunction(IntrinsicFunctionAstNode intrinsicFunctionAstNode)
    {
        CheckCollection(intrinsicFunctionAstNode.Args, CheckFunctionArg);
        var returnTypeRef = intrinsicFunctionAstNode.ReturnTypeRef;
        if (returnTypeRef == null)
        {
            return null;
        }

        var returnType = returnTypeRef.Resolve();
        if (returnType == null)
        {
            errorManager.Report(Error.UnresolvedType(returnTypeRef.Name));
        }

        return null;
    }

    protected override AstType? CheckFunctionArg(FunctionArgAstNode functionArgAstNode)
    {
        var typeRef = functionArgAstNode.TypeReference;
        if (typeRef == null)
        {
            return SimpleAstType.Any;
        }

        var type = typeRef.Resolve();
        if (type == null)
        {
            errorManager.Report(Error.UnresolvedType(typeRef.Name));
            return null;
        }

        return type;
    }

    protected override AstType? CheckNew(NewAstNode newAstNode)
    {
        // todo: check constructor
        var typeRef = newAstNode.TypeReference;
        var type = typeRef.Resolve();
        if (type == null)
        {
            errorManager.Report(Error.UnresolvedType(typeRef.Name));
            return null;
        }

        return type;
    }

    protected override AstType? CheckExpression(IExpressionAstNode expressionAstNode)
    {
        switch (expressionAstNode)
        {
            case BinaryExpressionAstNode binaryExpressionAstNode:
                var leftType = CheckExpression(binaryExpressionAstNode.Left);
                if (leftType == null)
                {
                    return null;
                }

                var rightType = CheckExpression(binaryExpressionAstNode.Right);
                if (rightType == null)
                {
                    return null;
                }

                switch (binaryExpressionAstNode.Op)
                {
                    case BinaryOperation.Mul:
                    case BinaryOperation.Div:
                    case BinaryOperation.Mod:
                    case BinaryOperation.Sum:
                    case BinaryOperation.Sub:
                        return CheckBinArithmeticExpression(leftType, rightType);

                    case BinaryOperation.LtEq:
                    case BinaryOperation.Lt:
                    case BinaryOperation.GtEq:
                    case BinaryOperation.Gt:
                        return CheckBinArithmeticBoolExpression(leftType, rightType);

                    case BinaryOperation.Eq:
                    case BinaryOperation.NotEq:
                        return CheckBinEqExpression(leftType, rightType);

                    case BinaryOperation.AndAnd:
                    case BinaryOperation.OrOr:
                    case BinaryOperation.Xor:
                        return CheckBinBoolExpression(leftType, rightType);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            case BoolLiteralAstNode:
                return SimpleAstType.Bool;
            case FloatLiteralAstNode:
                return SimpleAstType.Float;
            case IntLiteralAstNode:
                return SimpleAstType.Int;
            case StringLiteralAstNode:
                return SimpleAstType.StringT;

            case FunctionCallAstNode functionCallAstNode:
                return CheckFunctionCall(functionCallAstNode);
            case NewAstNode newAstNode:
                return CheckNew(newAstNode);
            case QualifiedAccessAstNodeBase qualifiedAccessAstNodeBase:
                return CheckQualifiedAccessBase(qualifiedAccessAstNodeBase);

            case UnaryExpressionAstNode unaryExpressionAstNode:
                var type = CheckExpression(unaryExpressionAstNode.Value);
                if (type == null)
                {
                    return null;
                }

                switch (unaryExpressionAstNode.Op)
                {
                    case UnaryOperation.NOT:
                        if (!CheckTypes(SimpleAstType.Bool, type))
                        {
                            errorManager.Report(Error.TypeMissmatch(SimpleAstType.Bool, type));
                            return null;
                        }
                        break;
                    case UnaryOperation.MINUS:
                    {
                        if (!_numericTypes.Contains(type))
                        {
                            errorManager.Report(Error.NumericTypeExpected(type));
                            return null;
                        }
                        break;
                    }
                }

                return type;
            case VarExpressionAstNode varExpressionAstNode:
                var varRef = varExpressionAstNode.VariableReference;
                var var = varRef.Resolve();
                if (var == null)
                {
                    errorManager.Report(Error.UnresolvedType(varRef.Name));
                    return null;
                }

                var varTypeRef = var.TypeReference;
                if (varTypeRef == null)
                {
                    return SimpleAstType.Any;
                }

                var varType = varTypeRef.Resolve();
                if (varType == null)
                {
                    errorManager.Report(Error.UnresolvedType(varTypeRef.Name));
                    return null;
                }

                return varType;
            default:
                throw new ArgumentOutOfRangeException(nameof(expressionAstNode));
        }
    }

    private AstType? CheckBinArithmeticExpression(AstType leftType, AstType rightType)
    {
        if (!CheckTypes(leftType, rightType))
        {
            errorManager.Report(Error.TypeMissmatch(leftType, rightType));
            return null;
        }

        if (!_numericTypes.Contains(leftType))
        {
            errorManager.Report(Error.NumericTypeExpected(leftType));
        }

        if (!_numericTypes.Contains(rightType))
        {
            errorManager.Report(Error.NumericTypeExpected(leftType));
        }

        return leftType == SimpleAstType.Any ? rightType : leftType;
    }

    private AstType? CheckBinArithmeticBoolExpression(AstType leftType, AstType rightType)
    {
        if (!CheckTypes(leftType, rightType))
        {
            errorManager.Report(Error.TypeMissmatch(leftType, rightType));
            return null;
        }

        if (!_numericTypes.Contains(leftType))
        {
            errorManager.Report(Error.NumericTypeExpected(leftType));
        }

        if (!_numericTypes.Contains(rightType))
        {
            errorManager.Report(Error.NumericTypeExpected(leftType));
        }

        return SimpleAstType.Bool;
    }

    private AstType? CheckBinEqExpression(AstType leftType, AstType rightType)
    {
        if (CheckTypes(leftType, rightType))
        {
            return SimpleAstType.Bool;
        }

        errorManager.Report(Error.TypeMissmatch(leftType, rightType));
        return null;
    }

    private AstType? CheckBinBoolExpression(AstType leftType, AstType rightType)
    {
        if (leftType != SimpleAstType.Any && leftType != SimpleAstType.Bool)
        {
            errorManager.Report(Error.TypeMissmatch(SimpleAstType.Bool, leftType));
            return null;
        }

        if (rightType != SimpleAstType.Any && rightType != SimpleAstType.Bool)
        {
            errorManager.Report(Error.TypeMissmatch(SimpleAstType.Bool, rightType));
            return null;
        }

        return SimpleAstType.Bool;
    }

    private bool CheckTypes(AstType expected, AstType actual)
    {
        return expected is GenericType || expected == SimpleAstType.Any || expected == actual;
    }

    private bool CheckCollection<TE>(
        IEnumerable<TE> collection,
        Func<TE, AstType?> func,
        Action<TE> onError) where TE : IAstNode
    {
        var result = true;
        foreach (var astNode in collection)
        {
            if (func(astNode) == null)
            {
                onError(astNode);
                result = false;
            }
        }

        return result;
    }

    private bool CheckCollection<TE>(
        IEnumerable<TE> collection,
        Func<TE, AstType?> func) where TE : IAstNode => CheckCollection(collection, func, _ => { });
}
