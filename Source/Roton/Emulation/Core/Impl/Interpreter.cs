using System;
using System.Diagnostics;
using System.Linq;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Interpreter(IEngineAccessor engine, ITracer tracer) : IInterpreter
{
    private IEngine Engine
    {
        [DebuggerStepThrough] get => engine.Instance;
    }

    private ITracer Tracer
    {
        [DebuggerStepThrough] get => tracer;
    }

    public void Execute(IOopContext context)
    {
        Span<char> buffer = stackalloc char[256];
        var firstLine = true;

        while (true)
        {
            if (firstLine)
                firstLine = false;
            else
                Tracer?.TraceOop(context);

            context.Resume = false;
            context.Executed = true;

            var name = Engine.Parser.ReadWord(context.Index, context, buffer);
            if (name.Length == 0)
                break;

            var command = Engine.CommandList.Get(name);

            if (command != null)
            {
                command.Execute(context);
            }
            else
            {
                if (!Engine.BroadcastLabel(context.Index, name, false))
                {
                    if (name.IndexOf(':') < 0)
                    {
                        Engine.RaiseError($"Bad command {name.ToString()}");
                    }
                }
                else
                {
                    context.NextLine = false;
                }
            }

            if (context.Executed)
            {
                context.CommandsExecuted++;
                context.Executed = false;
            }
            else
            {
                context.Resume = true;
            }

            if (context.Resume)
            {
                context.Resume = false;
            }
            else
            {
                if (context.NextLine && context.Instruction > 0)
                {
                    Engine.Parser.DiscardLine(context.Index, context);
                }

                break;
            }
        }
    }
}