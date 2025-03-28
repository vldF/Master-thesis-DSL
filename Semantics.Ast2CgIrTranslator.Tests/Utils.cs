using System.Text;
using me.vldf.jsa.dsl.ir.builder.checkers;

namespace Semantics.Ast2CgIrTranslator.Tests;

public static class Utils
{
    public static string FormatTypeCheckerErrors(
        IReadOnlyCollection<ErrorCode> expectedTypeCheckErrors,
        IReadOnlyCollection<ErrorCode> actualTypeCheckErrors)
    {
        var missingErrors = expectedTypeCheckErrors.ToList();
        foreach (var actualTypeCheckError in actualTypeCheckErrors)
        {
            missingErrors.Remove(actualTypeCheckError);
        }

        var extraErrors = actualTypeCheckErrors.ToList();
        foreach (var expectedTypeCheckError in expectedTypeCheckErrors)
        {
            extraErrors.Remove(expectedTypeCheckError);
        }

        var error = new StringBuilder();
        error.AppendLine("missing errors:");
        foreach (var missingError in missingErrors)
        {
            error.AppendLine($" {missingError}");
        }

        error.AppendLine();
        error.AppendLine("extra errors:");
        foreach (var extraError in extraErrors)
        {
            error.AppendLine($" {extraError}");
        }

        return error.ToString();
    }
}
