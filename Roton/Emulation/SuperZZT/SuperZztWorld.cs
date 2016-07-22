using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class SuperZztWorld : IWorld
    {
        public SuperZztWorld(IMemory memory)
        {
            Memory = memory;
        }

        public override string ToString()
        {
            return Name;
        }

        private IMemory Memory { get; }

        public int Ammo
        {
            get { return Memory.Read16(0x784C); }
            set { Memory.Write16(0x784C, value); }
        }

        public int Board
        {
            get { return Memory.Read16(0x7859); }
            set { Memory.Write16(0x7859, value); }
        }

        public int EnergyCycles
        {
            get { return Memory.Read16(0x785D); }
            set { Memory.Write16(0x785D, value); }
        }

        public int Gems
        {
            get { return Memory.Read16(0x784E); }
            set { Memory.Write16(0x784E, value); }
        }

        public int Health
        {
            get { return Memory.Read16(0x7857); }
            set { Memory.Write16(0x7857, value); }
        }

        public IFlagList Flags => new SuperZztFlagList(Memory);

        public IKeyList Keys => new KeyList(Memory, 0x7850);

        public bool Locked
        {
            get { return Memory.ReadBool(0x79CC); }
            set { Memory.WriteBool(0x79CC, value); }
        }

        public string Name
        {
            get { return Memory.ReadString(0x7863); }
            set { Memory.WriteString(0x7863, value); }
        }

        public int Score
        {
            get { return Memory.Read16(0x7861); }
            set { Memory.Write16(0x7861, value); }
        }

        public int Stones
        {
            get { return Memory.Read16(0x79CD); }
            set { Memory.Write16(0x79CD, value); }
        }

        public int TimePassed
        {
            get { return Memory.Read16(0x79C8); }
            set { Memory.Write16(0x79C8, value); }
        }

        public int TorchCycles
        {
            get { return 0; }
            set { }
        }

        public int Torches
        {
            get { return 0; }
            set { }
        }

        public int WorldType => -2;
    }
}