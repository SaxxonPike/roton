using System.Collections.Generic;
using Roton.Core;

namespace Roton.Emulation.Models
{
    internal abstract class World : IWorld
    {
        public virtual int Ammo { get; set; }
        public virtual int Board { get; set; }
        public virtual int EnergyCycles { get; set; }
        public virtual IFlagList Flags { get; protected set; }
        public virtual int Gems { get; set; }
        public virtual int Health { get; set; }
        public virtual IKeyList Keys { get; protected set; }
        public virtual bool Locked { get; set; }
        public virtual string Name { get; set; }
        public virtual int Score { get; set; }
        public virtual int Stones { get; set; }
        public virtual int TimePassed { get; set; }
        public virtual int TorchCycles { get; set; }
        public virtual int Torches { get; set; }
        public abstract int WorldType { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}