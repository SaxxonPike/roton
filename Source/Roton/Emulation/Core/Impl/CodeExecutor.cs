using System;
using Roton.Emulation.Data;
using Roton.Emulation.Directions;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class CodeExecutor(
    IActorList actors,
    ITracer tracer,
    IParser parser,
    IObjectMover objectMover,
    IInterpreter interpreter,
    IFacts facts,
    IState state,
    IMessageHandler messageHandler,
    IBroadcaster broadcaster,
    IErrorRaiser errorRaiser,
    IActorRemover actorRemover,
    IDirectionEvaluator directionEvaluator,
    IScrollContent scrollContent)
    : ICodeExecutor
{
    public void ExecuteCode(int index, ref Word instruction, string name)
    {
        // In the original code, there's a series of "goto" statements. To preserve
        // the flow, we use nested loops.

        ref var oopByte = ref state.OopByte;

        while (true)
        {
            var context = new OopContext
            {
                Actor = actors[index],
                Index = index,
                Name = name,
                PreviousInstruction = instruction
            };

            // The code reference must be reacquired each iteration because it is
            // possible that the actor pointed to by "index" has changed.

            var code = actors.GetActorCode(index);

            while (true)
            {
                if (instruction < 0)
                    break;

                tracer?.TraceOop(ref context, ref instruction);

                // Command will contain the first character of a line.

                context.NextLine = true;
                context.PreviousInstruction = instruction;
                context.Command = ReadActorCodeByte(ref context, ref instruction, ref oopByte, code);

                // Skip labels.

                while (context.Command == ':')
                {
                    parser.DiscardLine(index, ref instruction);
                    tracer?.TraceOop(ref context, ref instruction);
                    context.Command = ReadActorCodeByte(ref context, ref instruction, ref oopByte, code);
                }

                switch (context.Command)
                {
                    case '\'':
                    case '@':
                    {
                        // Comments and object names have no effect when executed.

                        parser.DiscardLine(index, ref instruction);
                        break;
                    }
                    case '/':
                    case '?':
                    {
                        // Shorthand for #GO and #TRY.

                        if (context.Command == '/')
                            context.Repeat = true;

                        if (!directionEvaluator.TryEval(ref context, ref instruction, out var vector))
                        {
                            errorRaiser.RaiseError(ref context, "Bad direction");
                            break;
                        }

                        objectMover.ExecuteDirection(ref context, vector);

                        if (ReadActorCodeByte(ref context, ref instruction, ref oopByte, code) != '\r')
                            instruction--;

                        context.Moved = true;
                        break;
                    }
                    case '#':
                    {
                        // Commands go to the interpreter.

                        interpreter.Execute(ref context, ref instruction);
                        code = actors.GetActorCode(index);
                        break;
                    }
                    case '\r':
                    {
                        // Blank lines are included in the message content only if there is
                        // already message content pending.

                        if (scrollContent.LineCount > 0)
                            scrollContent.AddLine(string.Empty);
                        break;
                    }
                    case '\0':
                    {
                        // Not present in the code itself but returned by read functions
                        // to indicate the end of the code has been reached.

                        context.Finished = true;
                        break;
                    }
                    default:
                    {
                        // All other lines become message content.

                        scrollContent.AddLine($"{context.Command}{parser.ReadLine(context.Index, ref instruction)}");
                        break;
                    }
                }

                if (context.Finished ||
                    context.Moved ||
                    context.Repeat ||
                    context.Died ||
                    context.CommandsExecuted >= facts.MaxOopCommands)
                    break;
            }

            if (context.Repeat)
                instruction = context.PreviousInstruction;

            if (state.OopByte == 0)
                instruction = -1;

            if (scrollContent.LineCount > 0)
                ExecuteMessage(ref context);

            // If the player chooses a label in the message handler, it is immediately
            // executed. This is indicated by setting "Continue" to true. The context
            // resets when this happens.

            if (context.Continue)
                continue;

            if (context.Died)
                actorRemover.RemoveActor(context.Actor.Location, context.Index, context.DeathTile);

            break;
        }
    }

    private void ExecuteMessage(ref OopContext context)
    {
        var result = messageHandler.ExecuteMessage(ref context);
        if (result is { Cancelled: false, Shown: true, Label: not null })
            context.Continue = broadcaster.BroadcastLabel(context.Index, result.Label, false);
        scrollContent.ClearLines();
    }

    private static char ReadActorCodeByte(ref OopContext context, ref Word instruction, ref PChar oopByte,
        ReadOnlySpan<char> code)
    {
        var value = (char)0;

        if (instruction < 0 || instruction >= context.Actor.Length)
        {
            oopByte = default;
        }
        else
        {
            value = code[instruction];
            oopByte = value;
            instruction++;
        }

        return value;
    }
}