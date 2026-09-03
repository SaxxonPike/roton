using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CHANGE")]
[Context(Context.Super, "CHANGE")]
internal sealed class ChangeCommand(
    IElementList elementList,
    ITiles tiles,
    IErrorRaiser errorRaiser,
    IPlotter plotter,
    IKindEvaluator kindEvaluator)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        var success = false;

        if (kindEvaluator.TryEval(ref context, ref instruction, out var source))
        {
            if (kindEvaluator.TryEval(ref context, ref instruction, out var target))
            {
                var targetElement = elementList[target.Id];
                success = true;

                if (target.Color == 0 && targetElement.Color < 0xF0)
                    target.Color = targetElement.Color;

                var location = new Location(0, 1);

                while (tiles.FindTile(source, ref location))
                    plotter.Plot(location, target);
            }
        }

        if (!success)
            errorRaiser.RaiseError(ref context, "Bad #CHANGE");
    }
}