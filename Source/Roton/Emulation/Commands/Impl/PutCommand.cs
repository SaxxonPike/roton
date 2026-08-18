using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "PUT")]
[Context(Context.Super, "PUT")]
public sealed class PutCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        var vector = Engine.Parser.GetDirection(context);
        var success = false;

        if (vector is {} vec)
        {
            var kind = Engine.Parser.GetKind(context);
            if (kind is {} k)
            {
                success = true;
                    
                var target = context.Actor.Location + vec;
                Engine.PutTile(target, vec, k);
            }
        }

        if (!success)
            Engine.RaiseError("Bad #PUT");
    }
}