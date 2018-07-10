using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Draws;

namespace Roton.Emulation.Zzt
{
    public class ZztDrawList : DrawList
    {
        public ZztDrawList(ICollection<IDraw> actions) : base(new Dictionary<int, IDraw>
            {
                {0x0C, actions.OfType<DuplicatorDraw>().Single()},
                {0x0D, actions.OfType<BombDraw>().Single()},
                {0x0F, actions.OfType<StarDraw>().Single()},
                {0x10, actions.OfType<ClockwiseConveyorDraw>().Single()},
                {0x11, actions.OfType<CounterclockwiseConveyorDraw>().Single()},
                {0x1D, actions.OfType<BlinkWallDraw>().Single()},
                {0x1E, actions.OfType<TransporterDraw>().Single()},
                {0x1F, actions.OfType<LineWallDraw>().Single()},
                {0x24, actions.OfType<ObjectDraw>().Single()},
                {0x27, actions.OfType<SpinningGunDraw>().Single()},
                {0x28, actions.OfType<PusherDraw>().Single()},
            },
            actions.OfType<DefaultDraw>().Single())
        {
        }
    }
}