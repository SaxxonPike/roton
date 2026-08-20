using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class Tracer : ITracer
    {
        private long _stepNumber;
        private readonly List<TextWriter> _writers = [];

        public void TraceInput(EngineKeyCode keyCode)
        {
            if (_writers.Count == 0)
                return;

            foreach (var writer in _writers)
                writer.WriteLine($"{_stepNumber:D8}:    TRACE KEY  {keyCode}");
        }

        public void TraceOop(ref OopContext context, ref Word instruction)
        {
            if (_writers.Count == 0)
                return;

            var code = context.Actor.Code.Span;
            var offset = instruction;
            var end = instruction;

            if (code.IsEmpty)
                return;

            while (end < code.Length)
            {
                if (code[end] == 0x0D || code[end] == 0x00)
                    break;
                end++;
            }

            var line = code.Slice(offset, end - offset).ToString();
            foreach (var writer in _writers)
                writer.WriteLine($"{_stepNumber:D8}:{context.Index:D3} TRACE OOP  [{context.Actor}] {line}");
        }

        public void TraceStep()
        {
            _stepNumber++;
        }

        public void TraceBroadcast(int sender, ReadOnlySpan<char> term, int targetIndex, bool ignoreLock,
            bool ignoreSelfLock)
        {
            if (_writers.Count == 0)
                return;

            if (sender == targetIndex && !ignoreLock && !ignoreSelfLock)
                return;

            var options = new[]
            {
                ignoreLock ? "IgnoreLock" : string.Empty,
                ignoreSelfLock ? "IgnoreSelfLock" : string.Empty
            };

            var optionsString = string.Join(" ", options.Where(o => !string.IsNullOrEmpty(o)));

            foreach (var writer in _writers)
                writer.WriteLine(
                    $"{_stepNumber:D8}:{sender:D3} BROADCAST  {term.ToString()} -> {targetIndex}  {optionsString}");
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

        public void TraceError(ref OopContext context, ReadOnlySpan<char> message)
        {
            if (_writers.Count == 0)
                return;

            foreach (var writer in _writers)
                writer.WriteLine(
                    $"{_stepNumber:D8}:{context.Index:D3} ERROR      [{context.Actor}] {message.ToString()}");
        }
    }
}