using Roton.Core;

namespace Roton.Emulation.Conditions
{
    public class AnyCondition : ICondition
    {
        private readonly IEngine _engine;

        public AnyCondition(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "ANY";

        public bool? Execute(IOopContext context)
        {
            var kind = _engine.Parser.GetKind(context);
            if (kind == null)
                return null;

            return _engine.FindTile(kind, new Location(0, 1));
        }
    }
}