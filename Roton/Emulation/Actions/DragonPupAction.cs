using Roton.Emulation.Core;

namespace Roton.Emulation.Actions
{
    public class DragonPupAction : IAction
    {
        private readonly IEngine _engine;

        public DragonPupAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            _engine.UpdateBoard(_engine.Actors[index].Location);
        }
    }
}