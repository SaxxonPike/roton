using System;
using System.Diagnostics;
using System.Linq;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class Interpreter : IInterpreter
    {
        private readonly Lazy<IEngine> _engine;
        private readonly Lazy<ITracer> _tracer;
        
        public Interpreter(Lazy<IEngine> engine, Lazy<ITracer> tracer)
        {
            _engine = engine;
            _tracer = tracer;
        }

        private IEngine Engine
        {
            [DebuggerStepThrough] get => _engine.Value;
        }

        private ITracer Tracer
        {
            [DebuggerStepThrough] get => _tracer.Value;
        }

        public void Execute(IOopContext context)
        {
            var firstLine = true;
            
            while (true)
            {
                if (firstLine)
                    firstLine = false;
                else
                    Tracer?.TraceOop(context);

                context.Resume = false;
                context.Executed = true;

                var name = Engine.Parser.ReadWord(context.Index, context);
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
                        if (!name.Contains(':'))
                        {
                            Engine.RaiseError($"Bad command {name}");
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
                        Engine.Parser.ReadLine(context.Index, context);
                    }
                    break;
                }
            }
        }
    }
}