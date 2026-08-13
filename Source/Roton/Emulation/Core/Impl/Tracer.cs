using System.Collections.Generic;
using System.IO;
using System.Linq;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class Tracer : ITracer
    {
        private long _stepNumber;
        private readonly List<TextWriter> _writers = new List<TextWriter>();

        public void TraceInput(EngineKeyCode keyCode)
        {
            foreach (var writer in _writers)
                writer.WriteLine($"{_stepNumber:D8}:    TRACE KEY  {keyCode}");
        }

        public void TraceOop(IOopContext oopContext)
        {
            var code = oopContext.Actor.Code;
            var offset = oopContext.Instruction;
            var end = oopContext.Instruction;

            if (code == null)
                return;
            
            while (end < code.Length)
            {
                if (code[end] == 0x0D || code[end] == 0x00)
                    break;
                end++;
            }

            var line = code.Skip(offset).Take(end - offset).ToArray().ToStringValue();
            foreach (var writer in _writers)
                writer.WriteLine($"{_stepNumber:D8}:{oopContext.Index:D3} TRACE OOP  [{oopContext.Actor}] {line}");
        }

        public void TraceStep()
        {
            _stepNumber++;
        }

        public void TraceBroadcast(int sender, string term, int targetIndex, bool ignoreLock, bool ignoreSelfLock)
        {
            if (sender == targetIndex && !ignoreLock && !ignoreSelfLock)
                return;
            
            var options = new[]
            {
                ignoreLock ? "IgnoreLock" : string.Empty,
                ignoreSelfLock ? "IgnoreSelfLock" : string.Empty
            };

            var optionsString = string.Join(" ", options.Where(o => !string.IsNullOrEmpty(o)));

            foreach (var writer in _writers)
                writer.WriteLine($"{_stepNumber:D8}:{sender:D3} BROADCAST  {term} -> {targetIndex}  {optionsString}");
        }

        public void Attach(TextWriter writer)
        {
            if (!_writers.Contains(writer))
                _writers.Add(writer);
        }

        public void Detach(TextWriter writer)
        {
            _writers.Remove(writer);
        }
    }
}