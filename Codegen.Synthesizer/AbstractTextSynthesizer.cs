using System.Text;

namespace Codegen.Synthesizer;

public abstract class AbstractTextSynthesizer
{
    private StringBuilder _sb = new();
    private int _currentIndent = 0;
    private const int Indent = 4;
    private bool _isNewLine = true;

    public override string ToString()
    {
        return _sb.ToString();
    }

    public void IncreaserIndent()
    {
        _currentIndent += Indent;
    }

    public void DecreaseIndent()
    {
        _currentIndent -= Indent;
    }

    public void Append(string value)
    {
        if (_isNewLine)
        {
            AppendIndent();
        }

        _sb.Append(value);
        _isNewLine = value.EndsWith("\n");
    }

    private void AppendIndent()
    {
        for (var i = 0; i < _currentIndent; i++)
        {
            _sb.Append(' ');
        }
    }

    public void AppendLine(string value = "")
    {
        Append(value + "\n");
    }

    public void AppendSpace()
    {
        Append(" ");
    }
    public void AppendSemicolon()
    {
        AppendLine(";");
    }
}
