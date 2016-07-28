using System.Collections.Generic;
using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.ZZT
{
    internal sealed class ZztState : IState
    {
        private readonly IMemory _memory;

        public ZztState(IMemory memory, byte[] memoryBytes)
        {
            _memory = memory;
            memory.Write(0x0000, memoryBytes);
            BorderTile = new MemoryTile(_memory, 0x0072);
            Colors = new ZztColorList(_memory);
            DefaultActor = new Actor(_memory, 0x0076);
            EdgeTile = new MemoryTile(_memory, 0x0074);
            KeyVector = new MemoryVector(_memory, 0x7C68);
            LineChars = new ByteString(_memory, 0x0098);
            SoundBuffer = new Int16List(_memory, 0x7E91, 127);
            StarChars = new ByteString(_memory, 0x0336);
            TransporterHChars = new ByteString(_memory, 0x0236);
            TransporterVChars = new ByteString(_memory, 0x0136);
            Vector4 = new Int16List(_memory, 0x0062, 8);
            Vector8 = new Int16List(_memory, 0x0042, 16);
            Alerts = new ZztAlerts(_memory, Colors, 5);
            PlayerTimer = new MemoryTimer(_memory, 0x740A);
        }

        public int MainTime
        {
            get { return _memory.Read16(0x740A); }
            set { _memory.Write16(0x740A, value); }
        }

        public int VisibleTileCount
        {
            get { return _memory.Read16(0x4ACC); }
            set { _memory.Write16(0x4ACC, value); }
        }

        public bool AboutShown
        {
            get { return _memory.ReadBool(0x7A60); }
            set { _memory.WriteBool(0x7A60, value); }
        }

        public int ActIndex
        {
            get { return _memory.Read16(0x7406); }
            set { _memory.Write16(0x7406, value); }
        }

        public int ActorCount
        {
            get { return _memory.Read16(0x31CD); }
            set { _memory.Write16(0x31CD, value); }
        }

        public IAlerts Alerts { get; }

        public int BoardCount
        {
            get { return _memory.Read16(0x45BE); }
            set { _memory.Write16(0x45BE, value); }
        }

        public ITile BorderTile { get; }

        public bool BreakGameLoop
        {
            get { return _memory.ReadBool(0x4AC6); }
            set { _memory.WriteBool(0x4AC6, value); }
        }

        public bool CancelScroll
        {
            get { return _memory.ReadBool(0x7B66); }
            set { _memory.WriteBool(0x7B66, value); }
        }

        public IColorList Colors { get; }

        public IActor DefaultActor { get; }

        public string DefaultBoardName
        {
            get { return _memory.ReadString(0x241E); }
            set { _memory.WriteString(0x241E, value); }
        }

        public string DefaultSaveName
        {
            get { return _memory.ReadString(0x23EA); }
            set { _memory.WriteString(0x23EA, value); }
        }

        public string DefaultWorldName
        {
            get { return _memory.ReadString(0x2452); }
            set { _memory.WriteString(0x2452, value); }
        }

        public ITile EdgeTile { get; }

        public bool EditorMode
        {
            get { return _memory.ReadBool(0x740C); }
            set { _memory.WriteBool(0x740C, value); }
        }

        public int ForestIndex { get; set; }

        public int GameCycle
        {
            get { return _memory.Read16(0x7404); }
            set { _memory.Write16(0x7404, value); }
        }

        public bool GameOver
        {
            get { return _memory.ReadBool(0x7C8D); }
            set { _memory.WriteBool(0x7C8D, value); }
        }

        public bool GamePaused
        {
            get { return _memory.ReadBool(0x7408); }
            set { _memory.WriteBool(0x7408, value); }
        }

        public bool GameQuiet
        {
            get { return _memory.ReadBool(0x7C8C); }
            set { _memory.WriteBool(0x7C8C, value); }
        }

        public int GameSpeed
        {
            get { return _memory.Read8(0x4ACE); }
            set { _memory.Write8(0x4ACE, value); }
        }

        public int GameWaitTime
        {
            get { return _memory.Read16(0x7402); }
            set { _memory.Write16(0x7402, value); }
        }

        public bool Init
        {
            get { return _memory.ReadBool(0x7B60); }
            set { _memory.WriteBool(0x7B60, value); }
        }

        public bool KeyArrow
        {
            get { return _memory.ReadBool(0x7C7E); }
            set { _memory.WriteBool(0x7C7E, value); }
        }

        public int KeyPressed
        {
            get { return _memory.Read8(0x7C70); }
            set { _memory.Write8(0x7C70, value); }
        }

        public bool KeyShift
        {
            get { return _memory.ReadBool(0x7C6C); }
            set { _memory.WriteBool(0x7C6C, value); }
        }

        public IXyPair KeyVector { get; }

        public IList<int> LineChars { get; }

        public string Message
        {
            get { return _memory.ReadString(0x456E); }
            set { _memory.WriteString(0x456E, value); }
        }

        public string Message2 { get; set; }

        public int OopByte
        {
            get { return _memory.Read8(0x740E); }
            set { _memory.Write8(0x740E, value); }
        }

        public int OopNumber
        {
            get { return _memory.Read16(0x7426); }
            set { _memory.Write16(0x7426, value); }
        }

        public string OopWord
        {
            get { return _memory.ReadString(0x7410); }
            set { _memory.WriteString(0x7410, value); }
        }

        public int PlayerElement
        {
            get { return _memory.Read16(0x4AC8); }
            set { _memory.Write16(0x4AC8, value); }
        }

        public int PlayerTime
        {
            get { return _memory.Read16(0x4920); }
            set { _memory.Write16(0x4920, value); }
        }

        public ITimer PlayerTimer { get; }

        public bool QuitZzt
        {
            get { return _memory.ReadBool(0x4AC5); }
            set { _memory.WriteBool(0x4AC5, value); }
        }

        public IList<int> SoundBuffer { get; }

        public int SoundBufferLength
        {
            get { return _memory.Read8(0x7E90); }
            set { _memory.Write8(0x7E90, value); }
        }

        public bool SoundPlaying
        {
            get { return _memory.ReadBool(0x7F9A); }
            set { _memory.WriteBool(0x7F9A, value); }
        }

        public int SoundPriority
        {
            get { return _memory.Read16(0x7C8E); }
            set { _memory.Write16(0x7C8E, value); }
        }

        public int SoundTicks
        {
            get { return _memory.Read8(0x7E8F); }
            set { _memory.Write8(0x7E8F, value); }
        }

        public IList<int> StarChars { get; }

        public int StartBoard
        {
            get { return _memory.Read16(0x4ACA); }
            set { _memory.Write16(0x4ACA, value); }
        }

        public IList<int> TransporterHChars { get; }

        public IList<int> TransporterVChars { get; }

        public IList<int> Vector4 { get; }

        public IList<int> Vector8 { get; }

        public IList<int> WebChars { get; } = new List<int>();

        public string WorldFileName
        {
            get { return _memory.ReadString(0x23B6); }
            set { _memory.WriteString(0x23B6, value); }
        }

        public bool WorldLoaded
        {
            get { return _memory.ReadBool(0x7428); }
            set { _memory.WriteBool(0x7428, value); }
        }
    }
}