using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.SuperZzt
{
    public sealed class SuperZztWorld : IWorld
    {
        public SuperZztWorld(IMemory memory, IKeyList keyList)
        {
            Memory = memory;
            Keys = keyList;
        }

        private IMemory Memory { get; }

        public int Ammo
        {
            get { return Memory.Read16(0x784C); }
            set { Memory.Write16(0x784C, value); }
        }

        public int BoardIndex
        {
            get { return Memory.Read16(0x7859); }
            set { Memory.Write16(0x7859, value); }
        }

        public int EnergyCycles
        {
            get { return Memory.Read16(0x785D); }
            set { Memory.Write16(0x785D, value); }
        }

        public IFlags Flags => new SuperZztFlags(Memory);

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

        public bool IsLocked
        {
            get { return Memory.ReadBool(0x79CC); }
            set { Memory.WriteBool(0x79CC, value); }
        }

        public IKeyList Keys { get; }

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

        public override string ToString()
        {
            return Name;
        }
    }
}