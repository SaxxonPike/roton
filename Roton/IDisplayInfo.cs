using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public interface IDisplayInfo
    {
        int Ammo { get; }
        Location Camera { get; }
        AnsiChar Draw(Location location);
        IList<Element> Elements { get; }
        int EnergyCycles { get; }
        int GameSpeed { get; }
        int Gems { get; }
        int Health { get; set; }
        int Height { get; }
        int KeyPressed { get; }
        bool KeyShift { get; }
        IList<bool> Keys { get; }
        string Message { get; }
        string Message2 { get; }
        Actor Player { get; }
        bool Quiet { get; }
        void ReadInput();
        int ReadKey();
        int Score { get; }
        int Stones { get; }
        string StoneText { get; }
        ITerminal Terminal { get; }
        int TimeLimit { get; }
        int TimePassed { get; }
        bool TitleScreen { get; }
        int TorchCycles { get; }
        int Torches { get; }
        void WaitForTick();
        int Width { get; }
        string WorldName { get; }
    }
}
