using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CHANGE")]
[Context(Context.Super, "CHANGE")]
public sealed class ChangeCommand : ICommand
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public ChangeCommand(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Execute(IOopContext context)
    {
        var success = false;
        var source = Engine.Parser.GetKind(context);
        if (source != null)
        {
            var target = Engine.Parser.GetKind(context);
            if (target != null)
            {
                var targetElement = Engine.ElementList[target.Id];
                success = true;
                if (target.Color == 0 && targetElement.Color < 0xF0)
                {
                    target.Color = targetElement.Color;
                }
                var location = new Location(0, 1);
                while (Engine.Tiles.FindTile(source, location))
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