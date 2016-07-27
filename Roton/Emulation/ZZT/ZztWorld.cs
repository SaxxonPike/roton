using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.ZZT
{
    internal sealed class ZztWorld : IWorld
    {
        public ZztWorld(IMemory memory)
        {
            Memory = memory;
            TimeLimitTimer = new MemoryTimer(memory, 0x4920);
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

        public IFlagList Flags => new ZztFlags(Memory);

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

        public IKeyList Keys => new KeyList(Memory, 0x4822);

        public ITimer TimeLimitTimer { get; }

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