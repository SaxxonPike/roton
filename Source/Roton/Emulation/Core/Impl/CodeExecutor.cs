using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public class CodeExecutor(
    IActorList _actors,
    ITracer _tracer,
    IParser _parser,
    IObjectMover _objectMover,
    IInterpreter _interpreter,
    IFacts _facts,
    IState _state,
    ITileRemover _tileRemover,
    IMessageHandler _messageHandler,
    IBroadcaster _broadcaster,
    IErrorRaiser errorRaiser)
    : ICodeExecutor
{
    public void ExecuteCode(int index, ref Word instruction, string name)
    {
        var context = new OopContext(_actors)
        {
            Index = index,
            Name = name,
            PreviousInstruction = instruction
        };

        while (true)
        {
            if (instruction < 0)
                break;

            _tracer?.TraceOop(ref context, ref instruction);

            context.NextLine = true;
            context.PreviousInstruction = instruction;
            context.Command = ReadActorCodeByte(index, ref instruction);

            while (context.Command == ':')
            {
                _parser.DiscardLine(index, ref instruction);
                _tracer?.TraceOop(ref context, ref instruction);
                context.Command = ReadActorCodeByte(index, ref instruction);
            }

            switch (context.Command)
            {
                case '\'':
                case '@':
                    _parser.DiscardLine(index, ref instruction);
                    break;
                case '/':
                case '?':
                    if (context.Command == '/')
                        context.Repeat = true;

                    if (!_parser.TryEvalDirection(ref context, ref instruction, out var vector))
                    {
                        errorRaiser.RaiseError(ref context, "Bad direction");
                        break;
                    }

                    _objectMover.ExecuteDirection(ref context, vector);

                    if (ReadActorCodeByte(index, ref instruction) != '\r')
                        instruction--;
                    context.Moved = true;

                    break;
                case '#':
                    _interpreter.Execute(ref context, ref instruction);
                    break;
                case '\r':
                    if (context.HasMessage)
                        context.AddMessage(string.Empty);
                    break;
                case '\0':
                    context.Finished = true;
                    break;
                default:
                    context.AddMessage($"{context.Command}{_parser.ReadLine(context.Index, ref instruction)}");
                    break;
            }

            if (context.Finished ||
                context.Moved ||
                context.Repeat ||
                context.Died ||
                context.CommandsExecuted >= _facts.MaxOopCommands)
                break;
        }

        if (context.Repeat)
            instruction = context.PreviousInstruction;

        if (_state.OopByte == 0)
            instruction = -1;

        if (context.HasMessage)
            ExecuteMessage(ref context);

        if (context.Died)
            _tileRemover.RemoveActor(context.Actor.Location, context.Index, context.DeathTile);
    }

    private void ExecuteMessage(ref OopContext context)
    {
        var result = _messageHandler.ExecuteMessage(ref context);
        if (result is { Cancelled: false, Label: not null })
            context.NextLine = _broadcaster.BroadcastLabel(context.Index, result.Label, false);
    }

    private char ReadActorCodeByte(int index, ref Word instruction)
    {
        var actor = _actors[index];
        var value = (char)0;

        if (instruction < 0 || instruction >= actor.Length)
        {
            _state.OopByte = default;
        }
        else
        {
            value = actor.Code[instruction];
            _state.OopByte = value;
            instruction++;
        }

        return value;
    }
}