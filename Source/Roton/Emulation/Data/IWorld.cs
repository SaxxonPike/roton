﻿namespace Roton.Emulation.Data
{
    public interface IWorld
    {
        IFlags Flags { get; }
        IKeyList Keys { get; }
        int WorldType { get; }
        int Ammo { get; set; }
        int BoardIndex { get; set; }
        int EnergyCycles { get; set; }
        int Gems { get; set; }
        int Health { get; set; }
        bool IsLocked { get; set; }
        string Name { get; set; }
        int Score { get; set; }
        int Stones { get; set; }
        int TimePassed { get; set; }
        int TorchCycles { get; set; }
        int Torches { get; set; }
    }
}