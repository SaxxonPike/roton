namespace Roton.Emulation.Behavior
{
    public class BehaviorMapConfiguration : IBehaviorMapConfiguration
    {
        public int AmmoPerContainer { get; set; }
        public bool BuggyPassages { get; set; }
        public bool ForestToFloor { get; set; }
        public int HealthPerGem { get; set; }
        public bool MultiMovement { get; set; }
        public int ScorePerGem { get; set; }
    }
}