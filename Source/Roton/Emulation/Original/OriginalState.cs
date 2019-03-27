using System;
using System.Collections.Generic;
using System.Diagnostics;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalState : IState
    {
        private readonly Lazy<IMemory> _memory;
        private readonly Lazy<IHeap> _heap;
        private readonly Lazy<IEngineResourceService> _engineResourceService;

        public OriginalState(
            Lazy<IMemory> memory, 
            Lazy<IHeap> heap, 
            Lazy<IEngineResourceService> engineResourceService)
        {
            _memory = memory;
            _heap = heap;
            _engineResourceService = engineResourceService;

            Memory.Write(0x0000, EngineResourceService.GetMemoryData());
            BorderTile = new MemoryTile(Memory, 0x0072);
            DefaultActor = new Actor(Memory, Heap, 0x0076);
            EdgeTile = new MemoryTile(Memory, 0x0074);
            KeyVector = new MemoryVector(Memory, 0x7C68);
            LineChars = new ByteString(Memory, 0x0098);
            SoundBuffer = new SoundBufferList(memory, 0x7E90);
            StarChars = new ByteString(Memory, 0x0336);
            TransporterHChars = new ByteString(Memory, 0x0236);
            TransporterVChars = new ByteString(Memory, 0x0136);
            Vector4 = new Int16List(Memory, 0x0062, 8);
            Vector8 = new Int16List(Memory, 0x0042, 16);
        }

        private IMemory Memory
        {
            [DebuggerStepThrough] get => _memory.Value;
        }

        private IHeap Heap
        {
            [DebuggerStepThrough] get => _heap.Value;
        }

        private IEngineResourceService EngineResourceService
        {
            [DebuggerStepThrough] get => _engineResourceService.Value;
        }

        public int MainTime
        {
            get => Memory.Read16(0x740A);
            set => Memory.Write16(0x740A, value);
        }

        public int VisibleTileCount
        {
            get => Memory.Read16(0x4ACC);
            set => Memory.Write16(0x4ACC, value);
        }

        public bool AboutShown
        {
            get => Memory.ReadBool(0x7A60);
            set => Memory.WriteBool(0x7A60, value);
        }

        public int ActIndex
        {
            get => Memory.Read16(0x7406);
            set => Memory.Write16(0x7406, value);
        }

        public int ActorCount
        {
            get => Memory.Read16(0x31CD);
            set => Memory.Write16(0x31CD, value);
        }

        public int BoardCount
        {
            get => Memory.Read16(0x45BE);
            set => Memory.Write16(0x45BE, value);
        }

        public ITile BorderTile { get; }

        public bool BreakGameLoop
        {
            get => Memory.ReadBool(0x4AC6);
            set => Memory.WriteBool(0x4AC6, value);
        }

        public bool CancelScroll
        {
            get => Memory.ReadBool(0x7B66);
            set => Memory.WriteBool(0x7B66, value);
        }

        public IActor DefaultActor { get; }

        public string DefaultBoardName
        {
            get => Memory.ReadString(0x241E);
            set => Memory.WriteString(0x241E, value);
        }

        public string DefaultSaveName
        {
            get => Memory.ReadString(0x23EA);
            set => Memory.WriteString(0x23EA, value);
        }

        public string DefaultWorldName
        {
            get => Memory.ReadString(0x2452);
            set => Memory.WriteString(0x2452, value);
        }

        public ITile EdgeTile { get; }

        public bool EditorMode
        {
            get => Memory.ReadBool(0x740C);
            set => Memory.WriteBool(0x740C, value);
        }

        public int ForestIndex { get; set; }

        public int GameCycle
        {
            get => Memory.Read16(0x7404);
            set => Memory.Write16(0x7404, value);
        }

        public bool GameOver
        {
            get => Memory.ReadBool(0x7C8D);
            set => Memory.WriteBool(0x7C8D, value);
        }

        public bool GamePaused
        {
            get => Memory.ReadBool(0x7408);
            set => Memory.WriteBool(0x7408, value);
        }

        public bool GameQuiet
        {
            get => Memory.ReadBool(0x7C8C);
            set => Memory.WriteBool(0x7C8C, value);
        }

        public int GameSpeed
        {
            get => Memory.Read8(0x4ACE);
            set => Memory.Write8(0x4ACE, value);
        }

        public int GameWaitTime
        {
            get => Memory.Read16(0x7402);
            set => Memory.Write16(0x7402, value);
        }

        public bool Init
        {
            get => Memory.ReadBool(0x7B60);
            set => Memory.WriteBool(0x7B60, value);
        }

        public bool KeyArrow
        {
            get => Memory.ReadBool(0x7C7E);
            set => Memory.WriteBool(0x7C7E, value);
        }

        public EngineKeyCode KeyPressed
        {
            get => (EngineKeyCode) Memory.Read8(0x7C70);
            set => Memory.Write8(0x7C70, (int) value);
        }

        public bool KeyShift
        {
            get => Memory.ReadBool(0x7C6C);
            set => Memory.WriteBool(0x7C6C, value);
        }

        public IXyPair KeyVector { get; }

        public IReadOnlyList<int> LineChars { get; }

        public string Message
        {
            get => Memory.ReadString(0x456E);
            set => Memory.WriteString(0x456E, value);
        }

        public string Message2 { get; set; }

        public int OopByte
        {
            get => Memory.Read8(0x740E);
            set => Memory.Write8(0x740E, value);
        }

        public int OopNumber
        {
            get => Memory.Read16(0x7426);
            set => Memory.Write16(0x7426, value);
        }

        public string OopWord
        {
            get => Memory.ReadString(0x7410);
            set => Memory.WriteString(0x7410, value);
        }

        public int PlayerElement
        {
            get => Memory.Read16(0x4AC8);
            set => Memory.Write16(0x4AC8, value);
        }

        public int PlayerTime
        {
            get => Memory.Read16(0x4920);
            set => Memory.Write16(0x4920, value);
        }

        public bool QuitEngine
        {
            get => Memory.ReadBool(0x4AC5);
            set => Memory.WriteBool(0x4AC5, value);
        }

        public ISoundBufferList SoundBuffer { get; }

        public bool SoundPlaying
        {
            get => Memory.ReadBool(0x7F9A);
            set => Memory.WriteBool(0x7F9A, value);
        }

        public int SoundPriority
        {
            get => Memory.Read16(0x7C8E);
            set => Memory.Write16(0x7C8E, value);
        }

        public int SoundTicks
        {
            get => Memory.Read8(0x7E8F);
            set => Memory.Write8(0x7E8F, value);
        }

        public IReadOnlyList<int> StarChars { get; }

        public int StartBoard
        {
            get => Memory.Read16(0x4ACA);
            set => Memory.Write16(0x4ACA, value);
        }

        public IReadOnlyList<int> TransporterHChars { get; }

        public IReadOnlyList<int> TransporterVChars { get; }

        public IReadOnlyList<int> Vector4 { get; }

        public IReadOnlyList<int> Vector8 { get; }

        public IReadOnlyList<int> WebChars { get; } = new List<int>();

        public string WorldFileName
        {
            get => Memory.ReadString(0x23B6);
            set => Memory.WriteString(0x23B6, value);
        }

        public bool WorldLoaded
        {
            get => Memory.ReadBool(0x7428);
            set => Memory.WriteBool(0x7428, value);
        }
    }
}