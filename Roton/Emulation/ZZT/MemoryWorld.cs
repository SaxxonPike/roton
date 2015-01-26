﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    sealed internal class MemoryWorld : MemoryWorldBase
    {
        public MemoryWorld(Memory memory)
            : base(memory, -1)
        {
        }

        public override int Ammo
        {
            get
            {
                return Memory.Read16(0x481E);
            }
            set
            {
                Memory.Write16(0x481E, value);
            }
        }

        public override int Board
        {
            get
            {
                return Memory.Read16(0x482B);
            }
            set
            {
                Memory.Write16(0x482B, value);
            }
        }

        public override int EnergyCycles
        {
            get
            {
                return Memory.Read16(0x4831);
            }
            set
            {
                Memory.Write16(0x4831, value);
            }
        }

        public override int Gems
        {
            get
            {
                return Memory.Read16(0x4820);
            }
            set
            {
                Memory.Write16(0x4820, value);
            }
        }

        public override int Health
        {
            get
            {
                return Memory.Read16(0x4829);
            }
            set
            {
                Memory.Write16(0x4829, value);
            }
        }

        public override MemoryFlagArrayBase FlagMemory
        {
            get { return new MemoryFlagArray(Memory); }
        }

        public override MemoryKeyArray KeyMemory
        {
            get { return new MemoryKeyArray(Memory, 0x4822); }
        }

        public override bool Locked
        {
            get
            {
                return Memory.ReadBool(0x4922);
            }
            set
            {
                Memory.WriteBool(0x4922, value);
            }
        }

        public override string Name
        {
            get
            {
                return Memory.ReadString(0x4837);
            }
            set
            {
                Memory.WriteString(0x4837, value);
            }
        }

        public override int Score
        {
            get
            {
                return Memory.Read16(0x4835);
            }
            set
            {
                Memory.Write16(0x4835, value);
            }
        }

        public override int TimePassed
        {
            get
            {
                return Memory.Read16(0x491E);
            }
            set
            {
                Memory.Write16(0x491E, value);
            }
        }

        public override int TorchCycles
        {
            get
            {
                return Memory.Read16(0x482F);
            }
            set
            {
                Memory.Write16(0x482F, value);
            }
        }

        public override int Torches
        {
            get
            {
                return Memory.Read16(0x482D);
            }
            set
            {
                Memory.Write16(0x482D, value);
            }
        }

        public override int WorldType
        {
            get { return -1; }
        }
    }
}