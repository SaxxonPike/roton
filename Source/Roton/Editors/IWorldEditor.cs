using System;
using System.Collections.Generic;

namespace Roton.Editors;

public interface IWorldEditor
{
    int Ammo { get; set; }
    int Gems { get; set; }
    ISet<KeyColor> Keys { get; }
    int Health { get; set; }
    int StartBoard { get; set; }
    int Torches { get; set; }
    int TorchCycles { get; set; }
    int EnergyCycles { get; set; }
    int Score { get; set; }
    string Name { get; set; }
    IList<string> Flags { get; }
    TimeSpan TimePassed { get; set; }
    bool Locked { get; set; }

    IList<IBoardEditor> Boards { get; }
}