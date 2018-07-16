using System.Collections.Generic;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalState : IState
    {
        private readonly IMemory _memory;

        public OriginalState(IMemory memory, IEngineResourceService engineResourceService)
        {
            _memory = memory;
            memory.Write(0x0000, engineResourceService.GetMemoryData());
            BorderTile = new MemoryTile(_memory, 0x0072);
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
        }

        public int MainTime
        {
            get => _memory.Read16(0x740A);
            set => _memory.Write16(0x740A, value);
        }

        public int VisibleTileCount
        {
            get => _memory.Read16(0x4ACC);
            set => _memory.Write16(0x4ACC, value);
        }

        public bool AboutShown
        {
            get => _memory.ReadBool(0x7A60);
            set => _memory.WriteBool(0x7A60, value);
        }

        public int ActIndex
        {
            get => _memory.Read16(0x7406);
            set => _memory.Write16(0x7406, value);
        }

        public int ActorCount
        {
            get => _memory.Read16(0x31CD);
            set => _memory.Write16(0x31CD, value);
        }

        public int BoardCount
        {
            get => _memory.Read16(0x45BE);
            set => _memory.Write16(0x45BE, value);
        }

        public ITile BorderTile { get; }

        public bool BreakGameLoop
        {
            get => _memory.ReadBool(0x4AC6);
            set => _memory.WriteBool(0x4AC6, value);
        }

        public bool CancelScroll
        {
            get => _memory.ReadBool(0x7B66);
            set => _memory.WriteBool(0x7B66, value);
        }

        public IActor DefaultActor { get; }

        public string DefaultBoardName
        {
            get => _memory.ReadString(0x241E);
            set => _memory.WriteString(0x241E, value);
        }

        public string DefaultSaveName
        {
            get => _memory.ReadString(0x23EA);
            set => _memory.WriteString(0x23EA, value);
        }

        public string DefaultWorldName
        {
            get => _memory.ReadString(0x2452);
            set => _memory.WriteString(0x2452, value);
        }

        public ITile EdgeTile { get; }

        public bool EditorMode
        {
            get => _memory.ReadBool(0x740C);
            set => _memory.WriteBool(0x740C, value);
        }

        public int ForestIndex { get; set; }

        public int GameCycle
        {
            get => _memory.Read16(0x7404);
            set => _memory.Write16(0x7404, value);
        }

        public bool GameOver
        {
            get => _memory.ReadBool(0x7C8D);
            set => _memory.WriteBool(0x7C8D, value);
        }

        public bool GamePaused
        {
            get => _memory.ReadBool(0x7408);
            set => _memory.WriteBool(0x7408, value);
        }

        public bool GameQuiet
        {
            get => _memory.ReadBool(0x7C8C);
            set => _memory.WriteBool(0x7C8C, value);
        }

        public int GameSpeed
        {
            get => _memory.Read8(0x4ACE);
            set => _memory.Write8(0x4ACE, value);
        }

        public int GameWaitTime
        {
            get => _memory.Read16(0x7402);
            set => _memory.Write16(0x7402, value);
        }

        public bool Init
        {
            get => _memory.ReadBool(0x7B60);
            set => _memory.WriteBool(0x7B60, value);
        }

        public bool KeyArrow
        {
            get => _memory.ReadBool(0x7C7E);
            set => _memory.WriteBool(0x7C7E, value);
        }

        public EngineKeyCode KeyPressed
        {
            get => (EngineKeyCode) _memory.Read8(0x7C70);
            set => _memory.Write8(0x7C70, (int) value);
        }

        public bool KeyShift
        {
            get => _memory.ReadBool(0x7C6C);
            set => _memory.WriteBool(0x7C6C, value);
        }

        public IXyPair KeyVector { get; }

        public IList<int> LineChars { get; }

        public string Message
        {
            get => _memory.ReadString(0x456E);
            set => _memory.WriteString(0x456E, value);
        }

        public string Message2 { get; set; }

        public int OopByte
        {
            get => _memory.Read8(0x740E);
            set => _memory.Write8(0x740E, value);
        }

        public int OopNumber
        {
            get => _memory.Read16(0x7426);
            set => _memory.Write16(0x7426, value);
        }

        public string OopWord
        {
            get => _memory.ReadString(0x7410);
            set => _memory.WriteString(0x7410, value);
        }

        public int PlayerElement
        {
            get => _memory.Read16(0x4AC8);
            set => _memory.Write16(0x4AC8, value);
        }

        public int PlayerTime
        {
            get => _memory.Read16(0x4920);
            set => _memory.Write16(0x4920, value);
        }

        public bool QuitEngine
        {
            get => _memory.ReadBool(0x4AC5);
            set => _memory.WriteBool(0x4AC5, value);
        }

        public IList<int> SoundBuffer { get; }

        public int SoundBufferLength
        {
            get => _memory.Read8(0x7E90);
            set => _memory.Write8(0x7E90, value);
        }

        public bool SoundPlaying
        {
            get => _memory.ReadBool(0x7F9A);
            set => _memory.WriteBool(0x7F9A, value);
        }

        public int SoundPriority
        {
            get => _memory.Read16(0x7C8E);
            set => _memory.Write16(0x7C8E, value);
        }

        public int SoundTicks
        {
            get => _memory.Read8(0x7E8F);
            set => _memory.Write8(0x7E8F, value);
        }

        public IList<int> StarChars { get; }

        public int StartBoard
        {
            get => _memory.Read16(0x4ACA);
            set => _memory.Write16(0x4ACA, value);
        }

        public IList<int> TransporterHChars { get; }

        public IList<int> TransporterVChars { get; }

        public IList<int> Vector4 { get; }

        public IList<int> Vector8 { get; }

        public IList<int> WebChars { get; } = new List<int>();

        public string WorldFileName
        {
            get => _memory.ReadString(0x23B6);
            set => _memory.WriteString(0x23B6, value);
        }

        public bool WorldLoaded
        {
            get => _memory.ReadBool(0x7428);
            set => _memory.WriteBool(0x7428, value);
        }
    }
}