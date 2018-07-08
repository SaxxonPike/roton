namespace Roton.Emulation.Data.Impl
{
    public class Config : IConfig
    {
        public int RandomSeed { get; set; }
        public int AmmoPerPickup { get; set; }
        public bool ForestToFloor { get; set; }
        public int HealthPerGem { get; set; }
        public int ScorePerGem { get; set; }
        public bool BuggyPassages { get; set; }
        public bool MultiMovement { get; set; }
        public string ScrollMusic { get; set; }
        public string ScrollTitle { get; set; }
        public int ElementCount { get; set; }
        public bool BuggyPut { get; set; }
    }
}