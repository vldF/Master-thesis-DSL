using Ast.Builder.builder;

namespace Launcher;

class Program
{
    public static void Main()
    {
        var code = """
                   func TestFunc(arg1: Type1, arg2: Type2): ReturnType {
                        arg1 = arg2;
                        arg2 = arg2;
                   }
                   
                   object MyObject {
                        func InnerFunc(arg1: Type) {
                        }
                        
                        func InnerFunc2() { }
                   }
                   
                   """;
        var astBuilder = new AstBuilder();
        var file = astBuilder.FromString(code);
        Console.WriteLine(file.String());
    }
}
