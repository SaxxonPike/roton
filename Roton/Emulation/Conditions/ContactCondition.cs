using Roton.Core;
using Roton.Emulation.Core;
using Roton.Extensions;

namespace Roton.Emulation.Conditions
{
    public class ContactCondition : ICondition
    {
        private readonly IEngine _engine;

        public ContactCondition(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "CONTACT";

        public bool? Execute(IOopContext context)
        {
            var player = _engine.Player;
            var distance = new Location16(context.Actor.Location).Difference(player.Location);
            return distance.X * distance.X + distance.Y * distance.Y == 1;
        }
    }
}