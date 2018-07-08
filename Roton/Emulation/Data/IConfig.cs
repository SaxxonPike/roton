namespace Roton.Emulation.Data
{
    public interface IConfig
    {
        int RandomSeed { get; }
        int AmmoPerPickup { get; }
        bool ForestToFloor { get; }
        int HealthPerGem { get; }
        int ScorePerGem { get; }
        bool BuggyPassages { get; }
        bool MultiMovement { get; }
        string ScrollMusic { get; } // @"c-c+d-d+e-e+f-f+g-g"
        string ScrollTitle { get; } // @"Scroll"
        int ElementCount { get; }
        bool BuggyPut { get; }
    }
}