namespace Roton.Core
{
    public sealed class Context : IContext
    {
        private readonly IEngine _engine;

        private const int MaxGameCycle = 420;

        public Context(IEngine engine)
        {
            _engine = engine;
        }

        public void ExecuteOnce()
        {
            if (_engine.State.EditorMode)
            {
                // simulate a game cycle for visuals only
                _engine.State.ActIndex = 0;
                _engine.State.GameCycle++;
                if (_engine.State.GameCycle >= MaxGameCycle)
                {
                    _engine.State.GameCycle = 0;
                }

                foreach (var actor in _engine.Actors)
                {
                    if (actor.Cycle > 0 && _engine.State.ActIndex%actor.Cycle == _engine.State.GameCycle%actor.Cycle)
                    {
                        _engine.UpdateBoard(actor.Location);
                    }
                    _engine.State.ActIndex++;
                }
            }
        }

        public void Start() => _engine.Start();

        public void Stop() => _engine.Stop();
    }
}