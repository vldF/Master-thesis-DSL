using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes;

public record CgAnnotation(string Name, ICollection<ICgExpression> args) : ICgNode;
