using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CHANGE")]
[Context(Context.Super, "CHANGE")]
public sealed class ChangeCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        var success = false;

        if (Engine.Parser.TryEvalKind(ref context, ref instruction, out var source))
        {
            if (Engine.Parser.TryEvalKind(ref context, ref instruction, out var target))
            {
                var targetElement = Engine.Elements[target.Id];
                success = true;

                if (target.Color == 0 && targetElement.Color < 0xF0)
                    target.Color = targetElement.Color;

                var location = new Location(0, 1);

                while (Engine.Tiles.FindTile(source, ref location))
                    Engine.PlotTile(location, target);
            }
        }

        if (!success)
            Engine.RaiseError(ref context, "Bad #CHANGE");
    }
}