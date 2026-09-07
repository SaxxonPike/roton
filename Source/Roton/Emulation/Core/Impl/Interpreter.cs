using System;
using Roton.Emulation.Commands;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Interpreter(
    ITracer tracer,
    IParser parser,
    ICommandList commands,
    IBroadcaster broadcaster,
    IErrorRaiser errorRaiser)
    : IInterpreter
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];

        tracer.TraceOop(ref context, ref instruction);

        while (true)
        {
            context.Resume = false;
            context.Executed = true;

            var name = parser.ReadWord(context.Index, ref instruction, buffer);
            if (name.Length == 0)
            {
                // If the last character of a script is '#', the interpreter will
                // ordinarily hang. We detect and prevent an infinite loop.

                if (instruction >= context.Actor.Length - 1)
                {
                    tracer.TraceCrash("Last character of script is #");
                    context.Finished = true;
                }
                
                break;
            }

            var command = commands.Get(name);

            if (command != null)
            {
                command.Execute(ref context, ref instruction);
            }
            else
            {
                if (!broadcaster.BroadcastLabel(context.Index, name, false))
                {
                    if (name.IndexOf(':') < 0) 
                        errorRaiser.RaiseError(ref context, $"Bad command {name.ToString()}");
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
                if (context.NextLine && instruction > 0)
                {
                    parser.DiscardLine(context.Index, ref instruction);
                }

                break;
            }
        }
    }
}