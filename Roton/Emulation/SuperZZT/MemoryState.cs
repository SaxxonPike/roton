using System.Collections.Generic;
using Roton.Core;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class MemoryState : MemoryStateBase
    {
        private ITile _borderTile;
        private MemoryColorArray _colors;
        private MemoryActor _defaultActor;
        private MemoryTile _edgeTile;
        private MemoryVector _keyVector;
        private MemoryStringByteCollection _lineChars;
        private MemoryInt16Collection _soundBuffer;
        private MemoryStringByteCollection _starChars;
        private MemoryStringByteCollection _transporterHChars;
        private MemoryStringByteCollection _transporterVChars;
        private MemoryInt16Collection _vector4;
        private MemoryInt16Collection _vector8;
        private MemoryStringByteCollection _webChars;

        public MemoryState(Memory memory)
            : base(memory)
        {
            memory.Write(0x0000, Properties.Resources.szztextra);

            _borderTile = new Tile(0, 0); //Super ZZT doesn't keep this in game memory; it's in code
            _colors = new MemoryColorArray(Memory);
            _defaultActor = new MemoryActor(Memory, 0x2262);
            _edgeTile = new MemoryTile(Memory, 0x2260);
            _keyVector = new MemoryVector(Memory, 0xCC6E);
            _lineChars = new MemoryStringByteCollection(Memory, 0x22BA);
            _soundBuffer = new MemoryInt16Collection(Memory, 0xCF9F, 127);
            _starChars = new MemoryStringByteCollection(Memory, 0x2064);
            _transporterHChars = new MemoryStringByteCollection(Memory, 0x1F64);
            _transporterVChars = new MemoryStringByteCollection(Memory, 0x1E64);
            _vector4 = new MemoryInt16Collection(Memory, 0x2250, 8);
            _vector8 = new MemoryInt16Collection(Memory, 0x2230, 16);
            _webChars = new MemoryStringByteCollection(Memory, 0x227C);
        }

        public override int ActIndex
        {
            get { return Memory.Read16(0xB95A); }
            set { Memory.Write16(0xB95A, value); }
        }

        public override int ActorCount
        {
            get { return Memory.Read16(0x6AB3); }
            set { Memory.Write16(0x6AB3, value); }
        }

        public override bool AlertAmmo
        {
            get { return Memory.ReadBool(0x7C0B); }
            set { Memory.WriteBool(0x7C0B, value); }
        }

        public override bool AlertDark
        {
            get { return false; }
            set { }
        }

        public override bool AlertEnergy
        {
            get { return Memory.ReadBool(0x7C11); }
            set { Memory.WriteBool(0x7C11, value); }
        }

        public override bool AlertFake
        {
            get { return Memory.ReadBool(0x7C0F); }
            set { Memory.WriteBool(0x7C0F, value); }
        }

        public override bool AlertForest
        {
            get { return Memory.ReadBool(0x7C0E); }
            set { Memory.WriteBool(0x7C0E, value); }
        }

        public override bool AlertGem
        {
            get { return Memory.ReadBool(0x7C10); }
            set { Memory.WriteBool(0x7C10, value); }
        }

        public override bool AlertNoAmmo
        {
            get { return Memory.ReadBool(0x7C0C); }
            set { Memory.WriteBool(0x7C0C, value); }
        }

        public override bool AlertNoShoot
        {
            get { return Memory.ReadBool(0x7C0D); }
            set { Memory.WriteBool(0x7C0D, value); }
        }

        public override bool AlertNotDark
        {
            get { return false; }
            set { }
        }

        public override bool AlertNoTorch
        {
            get { return false; }
            set { }
        }

        public override bool AlertTorch
        {
            get { return false; }
            set { }
        }

        public override int BoardCount
        {
            get { return Memory.Read16(0x7784); }
            set { Memory.Write16(0x7784, value); }
        }

        public override ITile BorderTile
        {
            get { return _borderTile; }
            protected set { }
        }

        public override bool BreakGameLoop
        {
            get { return Memory.ReadBool(0x7C9E); }
            set { Memory.WriteBool(0x7C9E, value); }
        }

        public override IList<string> Colors
        {
            get { return _colors; }
            protected set { }
        }

        public override IActor DefaultActor
        {
            get { return _defaultActor; }
            protected set { }
        }

        public override string DefaultBoardName
        {
            get { return Memory.ReadString(0x2B32); }
            set { Memory.WriteString(0x2B32, value); }
        }

        public override string DefaultSaveName
        {
            get { return Memory.ReadString(0x2AF4); }
            set { Memory.WriteString(0x2AF4, value); }
        }

        public override string DefaultWorldName
        {
            get { return Memory.ReadString(0x2B70); }
            set { Memory.WriteString(0x2B70, value); }
        }

        public override ITile EdgeTile
        {
            get { return _edgeTile; }
            protected set { }
        }

        public override bool EditorMode
        {
            get { return Memory.ReadBool(0xB960); }
            set { Memory.WriteBool(0xB960, value); }
        }

        public override int ForestIndex
        {
            get { return Memory.Read16(0x2334); }
            set { Memory.Write16(0x2334, value); }
        }

        public override int GameCycle
        {
            get { return Memory.Read16(0xB958); }
            set { Memory.Write16(0xB958, value); }
        }

        public override bool GameOver
        {
            get { return Memory.ReadBool(0xCD9B); }
            set { Memory.WriteBool(0xCD9B, value); }
        }

        public override bool GamePaused
        {
            get { return Memory.ReadBool(0xB95C); }
            set { Memory.WriteBool(0xB95C, value); }
        }

        public override bool GameQuiet
        {
            get { return Memory.ReadBool(0xCD9A); }
            set { Memory.WriteBool(0xCD9A, value); }
        }

        public override int GameSpeed
        {
            get { return Memory.Read16(0x7CA4); }
            set { Memory.Write16(0x7CA4, value); }
        }

        public override int GameWaitTime
        {
            get { return Memory.Read16(0xB956); }
            set { Memory.Write16(0xB956, value); }
        }

        public override bool KeyArrow
        {
            get { return Memory.ReadBool(0xCC84); }
            set { Memory.WriteBool(0xCC84, value); }
        }

        public override int KeyPressed
        {
            get { return Memory.Read8(0xCC76); }
            set { Memory.Write8(0xCC76, value); }
        }

        public override bool KeyShift
        {
            get { return Memory.ReadBool(0xCC72); }
            set { Memory.WriteBool(0xCC72, value); }
        }

        public override IXyPair KeyVector
        {
            get { return _keyVector; }
            set { }
        }

        public override IList<int> LineChars
        {
            get { return _lineChars; }
            protected set { }
        }

        public override string Message
        {
            get { return Memory.ReadString(0x7C22); }
            set { Memory.WriteString(0x7C22, value); }
        }

        public override string Message2
        {
            get { return Memory.ReadString(0x7C60); }
            set { Memory.WriteString(0x7C60, value); }
        }

        public override int OopByte
        {
            get { return Memory.Read8(0xB962); }
            set { Memory.Write8(0xB962, value); }
        }

        public override int OopNumber
        {
            get { return Memory.Read16(0xB97A); }
            set { Memory.Write16(0xB97A, value); }
        }

        public override string OopWord
        {
            get { return Memory.ReadString(0xB964); }
            set { Memory.WriteString(0xB964, value); }
        }

        public override int PlayerElement
        {
            get { return Memory.Read16(0x7CA0); }
            set { Memory.Write16(0x7CA0, value); }
        }

        public override bool QuitZzt
        {
            get { return Memory.ReadBool(0x7C9D); }
            set { Memory.WriteBool(0x7C9D, value); }
        }

        public override IList<int> SoundBuffer
        {
            get { return _soundBuffer; }
            protected set { }
        }

        public override int SoundBufferLength
        {
            get { return Memory.Read8(0xCF9E); }
            set { Memory.Write8(0xCF9E, value); }
        }

        public override bool SoundPlaying
        {
            get { return Memory.ReadBool(0xD0A8); }
            set { Memory.WriteBool(0xD0A8, value); }
        }

        public override int SoundPriority
        {
            get { return Memory.Read16(0xCD9C); }
            set { Memory.Write16(0xCD9C, value); }
        }

        public override int SoundTicks
        {
            get { return Memory.Read8(0xCF9D); }
            set { Memory.Write8(0xCF9D, value); }
        }

        public override IList<int> StarChars
        {
            get { return _starChars; }
            protected set { }
        }

        public override int StartBoard
        {
            get { return Memory.Read16(0x7CA2); }
            set { Memory.Write16(0x7CA2, value); }
        }

        public override IList<int> TransporterHChars
        {
            get { return _transporterHChars; }
            protected set { }
        }

        public override IList<int> TransporterVChars
        {
            get { return _transporterVChars; }
            protected set { }
        }

        public override IList<int> Vector4
        {
            get { return _vector4; }
            protected set { }
        }

        public override IList<int> Vector8
        {
            get { return _vector8; }
            protected set { }
        }

        public override int VisibleTileCount
        {
            get { return 96*80; }
            set { }
        }

        public override IList<int> WebChars
        {
            get { return _webChars; }
            protected set { }
        }

        public override string WorldFileName
        {
            get { return Memory.ReadString(0x2AB6); }
            set { Memory.WriteString(0x2AB6, value); }
        }

        public override bool WorldLoaded
        {
            get { return Memory.ReadBool(0xB97C); }
            set { Memory.WriteBool(0xB97C, value); }
        }
    }
}