using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class InvisibleWallBehavior : ElementBehavior
    {
        private readonly IGrid _grid;
        private readonly IEngine _engine;
        private readonly ISounds _sounds;
        private readonly IElements _elements;
        private readonly IAlerts _alerts;
        
        public override string KnownName => KnownNames.Invisible;

        public InvisibleWallBehavior(IGrid grid, IEngine engine, ISounds sounds, IElements elements, IAlerts alerts)
        {
            _grid = grid;
            _engine = engine;
            _sounds = sounds;
            _elements = elements;
            _alerts = alerts;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _grid[location].Id = _elements.NormalId;
            _engine.UpdateBoard(location);
            _engine.PlaySound(3, _sounds.Invisible);
            _engine.SetMessage(0x64, _alerts.InvisibleMessage);
        }
    }
}