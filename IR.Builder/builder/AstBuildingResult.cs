using me.vldf.jsa.dsl.ir.builder.checkers;
using me.vldf.jsa.dsl.ir.nodes.declarations;

namespace me.vldf.jsa.dsl.ir.builder.builder;

public record AstBuildingResult(IReadOnlyCollection<FileAstNode>? Files, IReadOnlyCollection<Error>? Errors);
