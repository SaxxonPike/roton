using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [Context(Context.Original, "IDLE")]
    [Context(Context.Super, "IDLE")]
    public sealed class IdleCommand : ICommand
    {
        public void Execute(IOopContext context)
        {
            context.Moved = true;
        }
    }
}