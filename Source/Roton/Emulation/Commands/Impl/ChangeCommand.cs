using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CHANGE")]
[Context(Context.Super, "CHANGE")]
public sealed class ChangeCommand(
    IEngineAccessor engine,
    IElementList elementList,
    ITiles tiles,
    IParser parser)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        var success = false;

        if (parser.TryEvalKind(ref context, ref instruction, out var source))
        {
            if (parser.TryEvalKind(ref context, ref instruction, out var target))
            {
                var targetElement = elementList[target.Id];
                success = true;

                if (target.Color == 0 && targetElement.Color < 0xF0)
                    target.Color = targetElement.Color;

                var location = new Location(0, 1);

                while (tiles.FindTile(source, ref location))
                    Engine.PlotTile(location, target);
            }
        }

        if (!success)
            Engine.RaiseError(ref context, "Bad #CHANGE");
    }
}