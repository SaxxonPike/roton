using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class MemoryWorld : MemoryWorldBase
    {
        public MemoryWorld(IMemory memory)
            : base(memory)
        {
        }

        public override int Ammo
        {
            get { return Memory.Read16(0x784C); }
            set { Memory.Write16(0x784C, value); }
        }

        public override int Board
        {
            get { return Memory.Read16(0x7859); }
            set { Memory.Write16(0x7859, value); }
        }

        public override int EnergyCycles
        {
            get { return Memory.Read16(0x785D); }
            set { Memory.Write16(0x785D, value); }
        }

        public override int Gems
        {
            get { return Memory.Read16(0x784E); }
            set { Memory.Write16(0x784E, value); }
        }

        public override int Health
        {
            get { return Memory.Read16(0x7857); }
            set { Memory.Write16(0x7857, value); }
        }

        public override IFlagList Flags => new MemoryFlagArray(Memory);

        public override IKeyList Keys => new MemoryKeyArray(Memory, 0x7850);

        public override bool Locked
        {
            get { return Memory.ReadBool(0x79CC); }
            set { Memory.WriteBool(0x79CC, value); }
        }

        public override string Name
        {
            get { return Memory.ReadString(0x7863); }
            set { Memory.WriteString(0x7863, value); }
        }

        public override int Score
        {
            get { return Memory.Read16(0x7861); }
            set { Memory.Write16(0x7861, value); }
        }

        public override int Stones
        {
            get { return Memory.Read16(0x79CD); }
            set { Memory.Write16(0x79CD, value); }
        }

        public override int TimePassed
        {
            get { return Memory.Read16(0x79C8); }
            set { Memory.Write16(0x79C8, value); }
        }

        public override int WorldType => -2;
    }
}