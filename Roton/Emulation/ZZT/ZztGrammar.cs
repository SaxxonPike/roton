using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztGrammar : Grammar
    {
        protected override void PutTile(IEngine engine, IXyPair location, IXyPair vector, ITile kind)
        {
            // ZZT does not allow #put on the bottom row
            if (location.Y == 25)
                return;
            base.PutTile(engine, location, vector, kind);
        }

        public ZztGrammar(IColors colors, IElements elements, IWorld world, IBoard board, IEngine engine,
            ISounds sounds, IGrid grid, IActors actors, IFlags flags, IState state, IRandom random) : base(colors,
            elements, world, board, engine, sounds, grid, actors, flags, state, random)
        {
        }
    }
}