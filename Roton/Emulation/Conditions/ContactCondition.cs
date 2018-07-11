using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions
{
    [ContextEngine(ContextEngine.Zzt, "CONTACT")]
    [ContextEngine(ContextEngine.SuperZzt, "CONTACT")]
    public sealed class ContactCondition : ICondition
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