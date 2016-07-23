using System.Collections.Generic;
using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class SuperZztState : IState
    {
        private readonly IMemory _memory;

        public SuperZztState(IMemory memory, byte[] memoryBytes)
        {
            _memory = memory;
            memory.Write(0x0000, memoryBytes);

            BorderTile = new Tile(0, 0); //Super ZZT doesn't keep this in game memory; it's in code
            Colors = new SuperZztColorList(_memory);
            DefaultActor = new Actor(_memory, 0x2262);
            EdgeTile = new MemoryTile(_memory, 0x2260);
            KeyVector = new MemoryVector(_memory, 0xCC6E);
            LineChars = new ByteString(_memory, 0x22BA);
            SoundBuffer = new Int16List(_memory, 0xCF9F, 127);
            StarChars = new ByteString(_memory, 0x2064);
            TransporterHChars = new ByteString(_memory, 0x1F64);
            TransporterVChars = new ByteString(_memory, 0x1E64);
            Vector4 = new Int16List(_memory, 0x2250, 8);
            Vector8 = new Int16List(_memory, 0x2230, 16);
            WebChars = new ByteString(_memory, 0x227C);
            Alerts = new SuperZztAlerts(_memory, Colors);
        }

        public bool AboutShown { get; set; }

        public int ActIndex
        {
            get { return _memory.Read16(0xB95A); }
            set { _memory.Write16(0xB95A, value); }
        }

        public int ActorCount
        {
            get { return _memory.Read16(0x6AB3); }
            set { _memory.Write16(0x6AB3, value); }
        }

        public IAlerts Alerts { get; }

        public int BoardCount
        {
            get { return _memory.Read16(0x7784); }
            set { _memory.Write16(0x7784, value); }
        }

        public ITile BorderTile { get; }

        public bool BreakGameLoop
        {
            get { return _memory.ReadBool(0x7C9E); }
            set { _memory.WriteBool(0x7C9E, value); }
        }

        public bool CancelScroll { get; set; }

        public IColorList Colors { get; }

        public IActor DefaultActor { get; }

        public string DefaultBoardName
        {
            get { return _memory.ReadString(0x2B32); }
            set { _memory.WriteString(0x2B32, value); }
        }

        public string DefaultSaveName
        {
            get { return _memory.ReadString(0x2AF4); }
            set { _memory.WriteString(0x2AF4, value); }
        }

        public string DefaultWorldName
        {
            get { return _memory.ReadString(0x2B70); }
            set { _memory.WriteString(0x2B70, value); }
        }

        public ITile EdgeTile { get; }

        public bool EditorMode
        {
            get { return _memory.ReadBool(0xB960); }
            set { _memory.WriteBool(0xB960, value); }
        }

        public int ForestIndex
        {
            get { return _memory.Read16(0x2334); }
            set { _memory.Write16(0x2334, value); }
        }

        public int GameCycle
        {
            get { return _memory.Read16(0xB958); }
            set { _memory.Write16(0xB958, value); }
        }

        public bool GameOver
        {
            get { return _memory.ReadBool(0xCD9B); }
            set { _memory.WriteBool(0xCD9B, value); }
        }

        public bool GamePaused
        {
            get { return _memory.ReadBool(0xB95C); }
            set { _memory.WriteBool(0xB95C, value); }
        }

        public bool GameQuiet
        {
            get { return _memory.ReadBool(0xCD9A); }
            set { _memory.WriteBool(0xCD9A, value); }
        }

        public int GameSpeed
        {
            get { return _memory.Read16(0x7CA4); }
            set { _memory.Write16(0x7CA4, value); }
        }

        public int GameWaitTime
        {
            get { return _memory.Read16(0xB956); }
            set { _memory.Write16(0xB956, value); }
        }

        public bool Init { get; set; }

        public bool KeyArrow
        {
            get { return _memory.ReadBool(0xCC84); }
            set { _memory.WriteBool(0xCC84, value); }
        }

        public int KeyPressed
        {
            get { return _memory.Read8(0xCC76); }
            set { _memory.Write8(0xCC76, value); }
        }

        public bool KeyShift
        {
            get { return _memory.ReadBool(0xCC72); }
            set { _memory.WriteBool(0xCC72, value); }
        }

        public IXyPair KeyVector { get; }

        public IList<int> LineChars { get; }

        public int MainTime { get; set; }

        public string Message
        {
            get { return _memory.ReadString(0x7C22); }
            set { _memory.WriteString(0x7C22, value); }
        }

        public string Message2
        {
            get { return _memory.ReadString(0x7C60); }
            set { _memory.WriteString(0x7C60, value); }
        }

        public int OopByte
        {
            get { return _memory.Read8(0xB962); }
            set { _memory.Write8(0xB962, value); }
        }

        public int OopNumber
        {
            get { return _memory.Read16(0xB97A); }
            set { _memory.Write16(0xB97A, value); }
        }

        public string OopWord
        {
            get { return _memory.ReadString(0xB964); }
            set { _memory.WriteString(0xB964, value); }
        }

        public int PlayerElement
        {
            get { return _memory.Read16(0x7CA0); }
            set { _memory.Write16(0x7CA0, value); }
        }

        public int PlayerTime { get; set; }

        public bool QuitZzt
        {
            get { return _memory.ReadBool(0x7C9D); }
            set { _memory.WriteBool(0x7C9D, value); }
        }

        public IList<int> SoundBuffer { get; }

        public int SoundBufferLength
        {
            get { return _memory.Read8(0xCF9E); }
            set { _memory.Write8(0xCF9E, value); }
        }

        public bool SoundPlaying
        {
            get { return _memory.ReadBool(0xD0A8); }
            set { _memory.WriteBool(0xD0A8, value); }
        }

        public int SoundPriority
        {
            get { return _memory.Read16(0xCD9C); }
            set { _memory.Write16(0xCD9C, value); }
        }

        public int SoundTicks
        {
            get { return _memory.Read8(0xCF9D); }
            set { _memory.Write8(0xCF9D, value); }
        }

        public IList<int> StarChars { get; }

        public int StartBoard
        {
            get { return _memory.Read16(0x7CA2); }
            set { _memory.Write16(0x7CA2, value); }
        }

        public IList<int> TransporterHChars { get; }

        public IList<int> TransporterVChars { get; }

        public IList<int> Vector4 { get; }

        public IList<int> Vector8 { get; }

        public int VisibleTileCount
        {
            get { return 96*80; }
            set { }
        }

        public IList<int> WebChars { get; }

        public string WorldFileName
        {
            get { return _memory.ReadString(0x2AB6); }
            set { _memory.WriteString(0x2AB6, value); }
        }

        public bool WorldLoaded
        {
            get { return _memory.ReadBool(0xB97C); }
            set { _memory.WriteBool(0xB97C, value); }
        }
    }
}