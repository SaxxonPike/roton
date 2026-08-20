namespace Roton.Emulation.Data;

public interface IWorld
{
    IFlags Flags { get; }
    IKeyList Keys { get; }
    int WorldType { get; }
    ref Word Ammo { get; }
    ref Word BoardIndex { get; }
    ref Word EnergyCycles { get; }
    ref Word Gems { get; }
    ref Word Health { get; }
    ref Bool IsLocked { get; }
    string Name { get; set; }
    ref Word Score { get; }
    ref Word Stones { get; }
    ref Word TimePassed { get; }
    ref Word TorchCycles { get; }
    ref Word Torches { get; }
}