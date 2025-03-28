using System.Text;
using me.vldf.jsa.dsl.ir.builder.checkers;
using me.vldf.jsa.dsl.ir.nodes.declarations;

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

    public static void DumpIr(FileAstNode file, string resultDir)
    {
        var fileName = file.FileName!;
        var resultFilePath = resultDir + Path.DirectorySeparatorChar + fileName + ".ir";
        var resultPath = new DirectoryInfo(resultDir);
        if (!resultPath.Exists)
        {
            resultPath.Delete(true);
            resultPath.Create();
        }

        var str = file.String();

        File.WriteAllText(resultFilePath, str);
    }
}
