using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztGrammar : Grammar
    {
        public ZztGrammar(IColorList colors, IElements elements) : base(colors, elements)
        {
        }

        protected override void PutTile(IEngine engine, IXyPair location, IXyPair vector, ITile kind)
        {
            // ZZT does not allow #put on the bottom row
            if (location.Y == 25)
                return;
            base.PutTile(engine, location, vector, kind);
        }
    }
}