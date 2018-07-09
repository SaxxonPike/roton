using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Draws;

namespace Roton.Emulation.Behaviors
{
    public class DefaultDraw : IDraw
    {
        public AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0x3F, 0x40);
        }
    }
}