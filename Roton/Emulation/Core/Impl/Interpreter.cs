﻿using System.Linq;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class Interpreter : IInterpreter
    {
        private readonly IEngine _engine;

        public Interpreter(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            while (true)
            {
                context.Resume = false;
                context.Executed = true;

                var name = _engine.Parser.ReadWord(context.Index, context);
                if (name.Length == 0)
                    break;

                var command = _engine.CommandList.Get(name);

                if (command != null)
                {
                    command.Execute(context);
                }
                else
                {
                    if (!_engine.BroadcastLabel(context.Index, name, false))
                    {
                        if (!name.Contains(':'))
                        {
                            _engine.RaiseError($"Bad command {name}");
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
                        _engine.Parser.ReadLine(context.Index, context);
                    }
                    break;
                }
            }
        }
    }
}