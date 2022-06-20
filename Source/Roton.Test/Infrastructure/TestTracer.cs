using System.IO;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;

namespace Roton.Test.Infrastructure;

public class TestTracer : ITracer
{
    private readonly TextWriter _writer;
    private long _stepNumber;

    public TestTracer(TextWriter writer)
    {
        _writer = writer;
    }

    public void TraceInput(EngineKeyCode keyCode)
    {
        if (!Enabled)
            return;

        _writer.WriteLine($"{_stepNumber:D8}:    TRACE KEY  {keyCode}");
    }

    public void TraceOop(IOopContext oopContext)
    {
        if (!Enabled)
            return;

        var code = oopContext.Actor.Code;
        var offset = oopContext.Instruction;
        var end = oopContext.Instruction;
            
        while (end < code.Length)
        {
            if (code[end] == 0x0D || code[end] == 0x00)
                break;
            end++;
        }

        var line = code.Skip(offset).Take(end - offset).ToArray().ToStringValue();
            
        _writer.WriteLine($"{_stepNumber:D8}:{oopContext.Index:D3} TRACE OOP  [{oopContext.Actor}] {line}");
    }

    public void TraceStep()
    {
        if (!Enabled)
            return;

        _stepNumber++;
    }

    public bool Enabled { get; set; }
}