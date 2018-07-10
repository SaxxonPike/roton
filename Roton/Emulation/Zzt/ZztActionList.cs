using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Actions;

namespace Roton.Emulation.Zzt
{
    public class ZztActionList : ActionList
    {
        public ZztActionList(ICollection<IAction> actions) : base(new Dictionary<int, IAction>
            {
                {0x02, actions.OfType<MessengerAction>().Single()},
                {0x03, actions.OfType<MonitorAction>().Single()},
                {0x04, actions.OfType<PlayerAction>().Single()},
                {0x0A, actions.OfType<ScrollAction>().Single()},
                {0x0C, actions.OfType<DuplicatorAction>().Single()},
                {0x0D, actions.OfType<BombAction>().Single()},
                {0x0F, actions.OfType<StarAction>().Single()},
                {0x10, actions.OfType<ClockwiseConveyorAction>().Single()},
                {0x11, actions.OfType<CounterclockwiseConveyorAction>().Single()},
                {0x12, actions.OfType<BulletAction>().Single()},
                {0x1D, actions.OfType<BlinkWallAction>().Single()},
                {0x1E, actions.OfType<TransporterAction>().Single()},
                {0x22, actions.OfType<BearAction>().Single()},
                {0x23, actions.OfType<RuffianAction>().Single()},
                {0x24, actions.OfType<ObjectAction>().Single()},
                {0x25, actions.OfType<SlimeAction>().Single()},
                {0x26, actions.OfType<SharkAction>().Single()},
                {0x27, actions.OfType<SpinningGunAction>().Single()},
                {0x28, actions.OfType<PusherAction>().Single()},
                {0x29, actions.OfType<LionAction>().Single()},
                {0x2A, actions.OfType<TigerAction>().Single()},
                {0x2C, actions.OfType<CentipedeHeadAction>().Single()},
                {0x2D, actions.OfType<CentipedeSegmentAction>().Single()},
            },
            actions.OfType<DefaultAction>().Single())
        {
        }
    }
}