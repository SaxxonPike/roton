namespace Roton.Emulation.Behavior
{
    public interface IBehaviorMapConfiguration
    {
        int AmmoPerContainer { get; }
        bool BuggyPassages { get; }
        bool ForestToFloor { get; }
        int HealthPerGem { get; }
        bool MultiMovement { get; }
        int ScorePerGem { get; }
    }
}