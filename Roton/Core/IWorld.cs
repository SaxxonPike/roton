namespace Roton.Core
{
    public interface IWorld
    {
        int Ammo { get; set; }
        int Board { get; set; }
        int EnergyCycles { get; set; }
        IFlagList Flags { get; }
        int Gems { get; set; }
        int Health { get; set; }
        IKeyList Keys { get; }
        bool Locked { get; set; }
        string Name { get; set; }
        int Score { get; set; }
        int Stones { get; set; }
        int TimePassed { get; set; }
        int TorchCycles { get; set; }
        int Torches { get; set; }
        int WorldType { get; }
    }
}