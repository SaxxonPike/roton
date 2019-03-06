using System.IO;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;

namespace Roton.Test.Infrastructure
{
    public class TestTracer : ITracer
    {
        private readonly TextWriter _writer;

        public TestTracer(TextWriter writer)
        {
            _writer = writer;
        }

        public void TraceInput(EngineKeyCode keyCode)
        {
            _writer.WriteLine($"TRACE KEY  {keyCode}");
        }

        public void TraceOop(IOopContext oopContext)
        {
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
            
            _writer.WriteLine($"TRACE OOP  [{oopContext.Actor}] {line}");
        }
    }
}