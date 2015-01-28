using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    sealed internal class MemoryState : MemoryStateBase
    {
        MemoryTile _borderTile;
        MemoryColorArray _colors;
        MemoryActor _defaultActor;
        MemoryTile _edgeTile;
        MemoryVector _keyVector;
        MemoryStringByteCollection _lineChars;
        MemoryInt16Collection _soundBuffer;
        MemoryStringByteCollection _starChars;
        MemoryStringByteCollection _transporterHChars;
        MemoryStringByteCollection _transporterVChars;
        MemoryInt16Collection _vector4;
        MemoryInt16Collection _vector8;

        public MemoryState(Memory memory)
            : base(memory)
        {
            memory.Write(0x0000, Properties.Resources.zztextra);
            _borderTile = new MemoryTile(Memory, 0x0072);
            _colors = new MemoryColorArray(Memory);
            _defaultActor = new MemoryActor(Memory, 0x0076);
            _edgeTile = new MemoryTile(Memory, 0x0074);
            _keyVector = new MemoryVector(Memory, 0x7C68);
            _lineChars = new MemoryStringByteCollection(Memory, 0x0098);
            _soundBuffer = new MemoryInt16Collection(Memory, 0x7E91, 127);
            _starChars = new MemoryStringByteCollection(Memory, 0x0336);
            _transporterHChars = new MemoryStringByteCollection(Memory, 0x0236);
            _transporterVChars = new MemoryStringByteCollection(Memory, 0x0136);
            _vector4 = new MemoryInt16Collection(Memory, 0x0062, 8);
            _vector8 = new MemoryInt16Collection(Memory, 0x0042, 16);
        }

        public override bool AboutShown
        {
            get
            {
                return Memory.ReadBool(0x7A60);
            }
            set
            {
                Memory.WriteBool(0x7A60, value);
            }
        }

        public override int ActIndex
        {
            get
            {
                return Memory.Read16(0x7406);
            }
            set
            {
                Memory.Write16(0x7406, value);
            }
        }

        public override int ActorCount
        {
            get
            {
                return Memory.Read16(0x31CD);
            }
            set
            {
                Memory.Write16(0x31CD, value);
            }
        }

        public override bool AlertAmmo
        {
            get
            {
                return Memory.ReadBool(0x4AAB);
            }
            set
            {
                Memory.WriteBool(0x4AAB, value);
            }
        }

        public override bool AlertDark
        {
            get
            {
                return Memory.ReadBool(0x4AB1);
            }
            set
            {
                Memory.WriteBool(0x4AB1, value);
            }
        }

        public override bool AlertEnergy
        {
            get
            {
                return Memory.ReadBool(0x4AB5);
            }
            set
            {
                Memory.WriteBool(0x4AB5, value);
            }
        }

        public override bool AlertFake
        {
            get
            {
                return Memory.ReadBool(0x4AB3);
            }
            set
            {
                Memory.WriteBool(0x4AB3, value);
            }
        }

        public override bool AlertForest
        {
            get
            {
                return Memory.ReadBool(0x4AB2);
            }
            set
            {
                Memory.WriteBool(0x4AB2, value);
            }
        }

        public override bool AlertGem
        {
            get
            {
                return Memory.ReadBool(0x4AB4);
            }
            set
            {
                Memory.WriteBool(0x4AB4, value);
            }
        }

        public override bool AlertNoAmmo
        {
            get
            {
                return Memory.ReadBool(0x4AAC);
            }
            set
            {
                Memory.WriteBool(0x4AAC, value);
            }
        }

        public override bool AlertNoShoot
        {
            get
            {
                return Memory.ReadBool(0x4AAD);
            }
            set
            {
                Memory.WriteBool(0x4AAD, value);
            }
        }

        public override bool AlertNotDark
        {
            get
            {
                return Memory.ReadBool(0x4AB1);
            }
            set
            {
                Memory.WriteBool(0x4AB1, value);
            }
        }

        public override bool AlertNoTorch
        {
            get
            {
                return Memory.ReadBool(0x4AAF);
            }
            set
            {
                Memory.WriteBool(0x4AAF, value);
            }
        }

        public override bool AlertTorch
        {
            get
            {
                return Memory.ReadBool(0x4AAE);
            }
            set
            {
                Memory.WriteBool(0x4AAE, value);
            }
        }

        public override int BoardCount
        {
            get
            {
                return Memory.Read16(0x45BE);
            }
            set
            {
                Memory.Write16(0x45BE, value);
            }
        }

        public override Tile BorderTile
        {
            get
            {
                return _borderTile;
            }
            protected set
            {
            }
        }

        public override bool BreakGameLoop
        {
            get
            {
                return Memory.ReadBool(0x4AC6);
            }
            set
            {
                Memory.WriteBool(0x4AC6, value);
            }
        }

        public override bool CancelScroll
        {
            get
            {
                return Memory.ReadBool(0x7B66);
            }
            set
            {
                Memory.WriteBool(0x7B66, value);
            }
        }

        public override IList<string> Colors
        {
            get
            {
                return _colors;
            }
            protected set
            {
            }
        }

        public override Actor DefaultActor
        {
            get
            {
                return _defaultActor;
            }
            protected set
            {
            }
        }

        public override string DefaultBoardName
        {
            get
            {
                return Memory.ReadString(0x241E);
            }
            set
            {
                Memory.WriteString(0x241E, value);
            }
        }

        public override string DefaultSaveName
        {
            get
            {
                return Memory.ReadString(0x23EA);
            }
            set
            {
                Memory.WriteString(0x23EA, value);
            }
        }

        public override string DefaultWorldName
        {
            get
            {
                return Memory.ReadString(0x2452);
            }
            set
            {
                Memory.WriteString(0x2452, value);
            }
        }

        public override Tile EdgeTile
        {
            get
            {
                return _edgeTile;
            }
            protected set
            {
            }
        }

        public override bool EditorMode
        {
            get
            {
                return Memory.ReadBool(0x740C);
            }
            set
            {
                Memory.WriteBool(0x740C, value);
            }
        }

        public override int GameCycle
        {
            get
            {
                return Memory.Read16(0x7404);
            }
            set
            {
                Memory.Write16(0x7404, value);
            }
        }

        public override bool GameOver
        {
            get
            {
                return Memory.ReadBool(0x7C8D);
            }
            set
            {
                Memory.WriteBool(0x7C8D, value);
            }
        }

        public override bool GamePaused
        {
            get
            {
                return Memory.ReadBool(0x7408);
            }
            set
            {
                Memory.WriteBool(0x7408, value);
            }
        }

        public override bool GameQuiet
        {
            get
            {
                return Memory.ReadBool(0x7C8C);
            }
            set
            {
                Memory.WriteBool(0x7C8C, value);
            }
        }

        public override int GameSpeed
        {
            get
            {
                return Memory.Read8(0x4ACE);
            }
            set
            {
                Memory.Write8(0x4ACE, value);
            }
        }

        public override int GameWaitTime
        {
            get
            {
                return Memory.Read16(0x7402);
            }
            set
            {
                Memory.Write16(0x7402, value);
            }
        }

        public override bool Init
        {
            get
            {
                return Memory.ReadBool(0x7B60);
            }
            set
            {
                Memory.WriteBool(0x7B60, value);
            }
        }

        public override bool KeyArrow
        {
            get
            {
                return Memory.ReadBool(0x7C7E);
            }
            set
            {
                Memory.WriteBool(0x7C7E, value);
            }
        }

        public override int KeyPressed
        {
            get
            {
                return Memory.Read8(0x7C70);
            }
            set
            {
                Memory.Write8(0x7C70, value);
            }
        }

        public override bool KeyShift
        {
            get
            {
                return Memory.ReadBool(0x7C6C);
            }
            set
            {
                Memory.WriteBool(0x7C6C, value);
            }
        }

        public override Vector KeyVector
        {
            get
            {
                return _keyVector;
            }
            set
            {
                _keyVector.CopyFrom(value);
            }
        }

        public override IList<int> LineChars
        {
            get
            {
                return _lineChars;
            }
            protected set
            {
            }
        }

        public override string Message
        {
            get
            {
                return Memory.ReadString(0x456E);
            }
            set
            {
                Memory.WriteString(0x456E, value);
            }
        }

        public override int OOPByte
        {
            get
            {
                return Memory.Read8(0x740E);
            }
            set
            {
                Memory.Write8(0x740E, value);
            }
        }

        public override int OOPNumber
        {
            get
            {
                return Memory.Read16(0x7426);
            }
            set
            {
                Memory.Write16(0x7426, value);
            }
        }

        public override string OOPWord
        {
            get
            {
                return Memory.ReadString(0x7410);
            }
            set
            {
                Memory.WriteString(0x7410, value);
            }
        }

        public override int PlayerElement
        {
            get
            {
                return Memory.Read16(0x4AC8);
            }
            set
            {
                Memory.Write16(0x4AC8, value);
            }
        }

        public override bool QuitZZT
        {
            get
            {
                return Memory.ReadBool(0x4AC5);
            }
            set
            {
                Memory.WriteBool(0x4AC5, value);
            }
        }

        public override IList<int> SoundBuffer
        {
            get
            {
                return _soundBuffer;
            }
            protected set
            {
            }
        }

        public override int SoundBufferLength
        {
            get
            {
                return Memory.Read8(0x7E90);
            }
            set
            {
                Memory.Write8(0x7E90, value);
            }
        }

        public override bool SoundPlaying
        {
            get
            {
                return Memory.ReadBool(0x7F9A);
            }
            set
            {
                Memory.WriteBool(0x7F9A, value);
            }
        }

        public override int SoundPriority
        {
            get
            {
                return Memory.Read16(0x7C8E);
            }
            set
            {
                Memory.Write16(0x7C8E, value);
            }
        }

        public override int SoundTicks
        {
            get
            {
                return Memory.Read8(0x7E8F);
            }
            set
            {
                Memory.Write8(0x7E8F, value);
            }
        }

        public override IList<int> StarChars
        {
            get
            {
                return _starChars;
            }
            protected set
            {
            }
        }

        public override int StartBoard
        {
            get
            {
                return Memory.Read16(0x4ACA);
            }
            set
            {
                Memory.Write16(0x4ACA, value);
            }
        }

        public override IList<int> TransporterHChars
        {
            get
            {
                return _transporterHChars;
            }
            protected set
            {
            }
        }

        public override IList<int> TransporterVChars
        {
            get
            {
                return _transporterVChars;
            }
            protected set
            {
            }
        }

        public override IList<int> Vector4
        {
            get
            {
                return _vector4;
            }
            protected set
            {
            }
        }

        public override IList<int> Vector8
        {
            get
            {
                return _vector8;
            }
            protected set
            {
            }
        }

        public override int VisibleTileCount
        {
            get
            {
                return Memory.Read16(0x4ACC);
            }
            set
            {
                Memory.Write16(0x4ACC, value);
            }
        }

        public override string WorldFileName
        {
            get
            {
                return Memory.ReadString(0x23B6);
            }
            set
            {
                Memory.WriteString(0x23B6, value);
            }
        }

        public override bool WorldLoaded
        {
            get
            {
                return Memory.ReadBool(0x7428);
            }
            set
            {
                Memory.WriteBool(0x7428, value);
            }
        }
    }
}
