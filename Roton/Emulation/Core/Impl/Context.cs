using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class Context : IContext
    {
        private readonly IFacts _facts;

        public Context(IEngine engine, IFacts facts)
        {
            Engine = engine;
            _facts = facts;
        }

        public IEngine Engine { get; }
        
        public void ExecuteOnce()
        {
            if (Engine.State.EditorMode)
            {
                // simulate a game cycle for visuals only
                Engine.State.ActIndex = 0;
                Engine.State.GameCycle++;
                if (Engine.State.GameCycle >= _facts.MaxGameCycle)
                {
                    Engine.State.GameCycle = 0;
                }

                foreach (var actor in Engine.Actors)
                {
                    if (actor.Cycle > 0 && Engine.State.ActIndex%actor.Cycle == Engine.State.GameCycle%actor.Cycle)
                    {
                        Engine.UpdateBoard(actor.Location);
                    }
                    Engine.State.ActIndex++;
                }
            }
        }

        public void Start() => Engine.Start();

        public void Stop() => Engine.Stop();
    }
}