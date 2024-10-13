using me.vldf.jsa.dsl.parser.ast.builder;

namespace Launcher;

class Program
{
    public static void Main()
    {
        var code = """
                   func TestFunc(arg1: Type1, arg2: Type2): ReturnType {
                        expr; expr;
                        expr; expr;
                   }
                   
                   object MyObject {
                        func InnerFunc(arg1: Type) {
                            expr;
                        }
                        
                        func InnerFunc2() { expr; }
                   }
                   
                   """;
        var astBuilder = new AstBuilder();
        var file = astBuilder.FromString(code);
        Console.WriteLine(file.String());
    }
}
