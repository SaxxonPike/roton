using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl
{
    [Context(Context.Original, "I")]
    [Context(Context.Original, "IDLE")]
    [Context(Context.Super, "I")]
    [Context(Context.Super, "IDLE")]
    public sealed class IdleDirection : IDirection
    {
        public IXyPair Execute(IOopContext context)
        {
            return Vector.Idle;
        }
    }
}