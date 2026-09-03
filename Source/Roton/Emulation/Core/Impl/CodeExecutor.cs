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
    IDirectionEvaluator directionEvaluator)
    : ICodeExecutor
{
    public void ExecuteCode(int index, ref Word instruction, string name)
    {
        var context = new OopContext
        {
            Actor = actors[index],
            Index = index,
            Name = name,
            PreviousInstruction = instruction
        };

        while (true)
        {
            if (instruction < 0)
                break;

            tracer?.TraceOop(ref context, ref instruction);

            context.NextLine = true;
            context.PreviousInstruction = instruction;
            context.Command = ReadActorCodeByte(index, ref instruction);

            while (context.Command == ':')
            {
                parser.DiscardLine(index, ref instruction);
                tracer?.TraceOop(ref context, ref instruction);
                context.Command = ReadActorCodeByte(index, ref instruction);
            }

            switch (context.Command)
            {
                case '\'':
                case '@':
                    parser.DiscardLine(index, ref instruction);
                    break;
                case '/':
                case '?':
                    if (context.Command == '/')
                        context.Repeat = true;

                    if (!directionEvaluator.TryEval(ref context, ref instruction, out var vector))
                    {
                        errorRaiser.RaiseError(ref context, "Bad direction");
                        break;
                    }

                    objectMover.ExecuteDirection(ref context, vector);

                    if (ReadActorCodeByte(index, ref instruction) != '\r')
                        instruction--;
                    context.Moved = true;

                    break;
                case '#':
                    interpreter.Execute(ref context, ref instruction);
                    break;
                case '\r':
                    if (context.HasMessage)
                        context.AddMessage(string.Empty);
                    break;
                case '\0':
                    context.Finished = true;
                    break;
                default:
                    context.AddMessage($"{context.Command}{parser.ReadLine(context.Index, ref instruction)}");
                    break;
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

        if (context.HasMessage)
            ExecuteMessage(ref context);

        if (context.Died)
            actorRemover.RemoveActor(context.Actor.Location, context.Index, context.DeathTile);
    }

    private void ExecuteMessage(ref OopContext context)
    {
        var result = messageHandler.ExecuteMessage(ref context);
        if (result is { Cancelled: false, Shown: true, Label: not null })
            context.NextLine = broadcaster.BroadcastLabel(context.Index, result.Label, false);
    }

    private char ReadActorCodeByte(int index, ref Word instruction)
    {
        var actor = actors[index];
        var value = (char)0;

        if (instruction < 0 || instruction >= actor.Length)
        {
            state.OopByte = default;
        }
        else
        {
            value = actor.Code[instruction];
            state.OopByte = value;
            instruction++;
        }

        return value;
    }
}