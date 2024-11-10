using Codegen.Synthesizer;
using me.vldf.jsa.dsl.ir.builder.builder;
using Semantics.Ast2CgIrTranslator;

namespace Launcher;

class Program
{
    public static void Main()
    {
        var code = """
                   object Type1 {}
                   object Type2 {}
                   object ReturnType {}
                   
                   func TestFunc(arg1: Type1, arg2: Type2): ReturnType {
                        arg1 = arg2;
                        arg2 = arg2;
                        
                        return arg1;
                   }
                   
                   object MyObject {
                        func InnerFunc(arg1: Type1) {
                        }
                        
                        func InnerFunc2() { }
                   }
                   """;

        var astBuilder = new AstBuilder();
        var file = astBuilder.FromString(code);
        Console.WriteLine(file.String());

        var translator = new Ast2IrTranslator();
        var cgFile = translator.Translate(file);

        var synth = new CodegenSynthesizer();
        var result = synth.Synthesize(cgFile);

        Console.WriteLine("=======");
        Console.WriteLine(result);
    }
}
