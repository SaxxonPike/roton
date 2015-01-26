using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Roton
{
    abstract public partial class World
    {
        virtual public int Ammo { get; set; }
        virtual public int Board { get; set; }
        virtual public int EnergyCycles { get; set; }
        virtual public IList<string> Flags { get; protected set; }
        virtual public int Gems { get; set; }
        virtual public int Health { get; set; }
        virtual public IList<bool> Keys { get; protected set; }
        virtual public bool Locked { get; set; }
        virtual public string Name { get; set; }
        virtual public int Score { get; set; }
        virtual public int Stones { get; set; }
        virtual public int TimePassed { get; set; }
        virtual public int TorchCycles { get; set; }
        virtual public int Torches { get; set; }
        abstract public int WorldType { get; }
    }
}
