using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CHANGE")]
[Context(Context.Super, "CHANGE")]
public sealed class ChangeCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        var success = false;
        var sourceVal = Engine.Parser.GetKind(ref context, ref instruction);
        if (sourceVal is { } source)
        {
            var targetVal = Engine.Parser.GetKind(ref context, ref instruction);
            if (targetVal is {} target)
            {
                var targetElement = Engine.ElementList[target.Id];
                success = true;
                if (target.Color == 0 && targetElement.Color < 0xF0)
                {
                    target.Color = targetElement.Color;
                }
                var location = new Location(0, 1);
                while (Engine.Tiles.FindTile(source, ref location))
                {
                    Engine.PlotTile(location, target);
                }
            }
        }

        if (!success)
        {
            Engine.RaiseError("Bad #CHANGE");
        }
    }
}