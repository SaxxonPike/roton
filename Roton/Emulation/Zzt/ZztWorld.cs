using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztWorld : IWorld
    {
        public ZztWorld(IMemory memory, IKeyList keyList)
        {
            Memory = memory;
            Keys = keyList;
        }

        private IMemory Memory { get; }

        public int Ammo
        {
            get { return Memory.Read16(0x481E); }
            set { Memory.Write16(0x481E, value); }
        }

        public int BoardIndex
        {
            get { return Memory.Read16(0x482B); }
            set { Memory.Write16(0x482B, value); }
        }

        public int EnergyCycles
        {
            get { return Memory.Read16(0x4831); }
            set { Memory.Write16(0x4831, value); }
        }

        public int Gems
        {
            get { return Memory.Read16(0x4820); }
            set { Memory.Write16(0x4820, value); }
        }

        public int Health
        {
            get { return Memory.Read16(0x4829); }
            set { Memory.Write16(0x4829, value); }
        }

        public bool IsLocked
        {
            get { return Memory.ReadBool(0x4922); }
            set { Memory.WriteBool(0x4922, value); }
        }

        public IKeyList Keys { get; }

        public string Name
        {
            get { return Memory.ReadString(0x4837); }
            set { Memory.WriteString(0x4837, value); }
        }

        public int Score
        {
            get { return Memory.Read16(0x4835); }
            set { Memory.Write16(0x4835, value); }
        }

        public int Stones
        {
            get { return 0; }
            set { }
        }

        public int TimePassed
        {
            get { return Memory.Read16(0x491E); }
            set { Memory.Write16(0x491E, value); }
        }

        public int TorchCycles
        {
            get { return Memory.Read16(0x482F); }
            set { Memory.Write16(0x482F, value); }
        }

        public int Torches
        {
            get { return Memory.Read16(0x482D); }
            set { Memory.Write16(0x482D, value); }
        }

        public int WorldType => -1;
    }
}