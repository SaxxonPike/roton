using System;
using System.Collections.Generic;
using System.Diagnostics;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperState : IState
    {
        private readonly Lazy<IMemory> _memory;
        private readonly Lazy<IEngineResourceService> _engineResourceService;
        private readonly Lazy<IHeap> _heap;

        public SuperState(Lazy<IMemory> memory, Lazy<IEngineResourceService> engineResourceService, Lazy<IHeap> heap)
        {
            _memory = memory;
            _engineResourceService = engineResourceService;
            _heap = heap;
            
            Memory.Write(0x0000, EngineResourceService.GetMemoryData());
            BorderTile = new Tile(0, 0); // Not in memory
            DefaultActor = new Actor(Memory, Heap, 0x2262);
            EdgeTile = new MemoryTile(Memory, 0x2260);
            KeyVector = new MemoryVector(Memory, 0xCC6E);
            LineChars = new ByteString(Memory, 0x22BA);
            SoundBuffer = new SoundBufferList(memory, 0xCF9E);
            StarChars = new ByteString(Memory, 0x2064);
            TransporterHChars = new ByteString(Memory, 0x1F64);
            TransporterVChars = new ByteString(Memory, 0x1E64);
            Vector4 = new Int16List(Memory, 0x2250, 8);
            Vector8 = new Int16List(Memory, 0x2230, 16);
            WebChars = new ByteString(Memory, 0x227C);
        }

        private IMemory Memory
        {
            [DebuggerStepThrough] get => _memory.Value;
        }

        private IEngineResourceService EngineResourceService
        {
            [DebuggerStepThrough] get => _engineResourceService.Value;
        }

        private IHeap Heap
        {
            [DebuggerStepThrough] get => _heap.Value;
        }

        public bool AboutShown { get; set; }

        public int ActIndex
        {
            get => Memory.Read16(0xB95A);
            set => Memory.Write16(0xB95A, value);
        }

        public int ActorCount
        {
            get => Memory.Read16(0x6AB3);
            set => Memory.Write16(0x6AB3, value);
        }

        public int BoardCount
        {
            get => Memory.Read16(0x7784);
            set => Memory.Write16(0x7784, value);
        }

        public ITile BorderTile { get; }

        public bool BreakGameLoop
        {
            get => Memory.ReadBool(0x7C9E);
            set => Memory.WriteBool(0x7C9E, value);
        }

        public bool CancelScroll { get; set; }

        public IActor DefaultActor { get; }

        public string DefaultBoardName
        {
            get => Memory.ReadString(0x2B32);
            set => Memory.WriteString(0x2B32, value);
        }

        public string DefaultSaveName
        {
            get => Memory.ReadString(0x2AF4);
            set => Memory.WriteString(0x2AF4, value);
        }

        public string DefaultWorldName
        {
            get => Memory.ReadString(0x2B70);
            set => Memory.WriteString(0x2B70, value);
        }

        public ITile EdgeTile { get; }

        public bool EditorMode
        {
            get => Memory.ReadBool(0xB960);
            set => Memory.WriteBool(0xB960, value);
        }

        public int ForestIndex
        {
            get => Memory.Read16(0x2334);
            set => Memory.Write16(0x2334, value);
        }

        public int GameCycle
        {
            get => Memory.Read16(0xB958);
            set => Memory.Write16(0xB958, value);
        }

        public bool GameOver
        {
            get => Memory.ReadBool(0xCD9B);
            set => Memory.WriteBool(0xCD9B, value);
        }

        public bool GamePaused
        {
            get => Memory.ReadBool(0xB95C);
            set => Memory.WriteBool(0xB95C, value);
        }

        public bool GameQuiet
        {
            get => Memory.ReadBool(0xCD9A);
            set => Memory.WriteBool(0xCD9A, value);
        }

        public int GameSpeed
        {
            get => Memory.Read16(0x7CA4);
            set => Memory.Write16(0x7CA4, value);
        }

        public int GameWaitTime
        {
            get => Memory.Read16(0xB956);
            set => Memory.Write16(0xB956, value);
        }

        public bool Init { get; set; }

        public bool KeyArrow
        {
            get => Memory.ReadBool(0xCC84);
            set => Memory.WriteBool(0xCC84, value);
        }

        public EngineKeyCode KeyPressed
        {
            get => (EngineKeyCode) Memory.Read8(0xCC76);
            set => Memory.Write8(0xCC76, (int) value);
        }

        public bool KeyShift
        {
            get => Memory.ReadBool(0xCC72);
            set => Memory.WriteBool(0xCC72, value);
        }

        public IXyPair KeyVector { get; }

        public IReadOnlyList<int> LineChars { get; }

        public string Message
        {
            get => Memory.ReadString(0x7C22);
            set => Memory.WriteString(0x7C22, value);
        }

        public string Message2
        {
            get => Memory.ReadString(0x7C60);
            set => Memory.WriteString(0x7C60, value);
        }

        public int OopByte
        {
            get => Memory.Read8(0xB962);
            set => Memory.Write8(0xB962, value);
        }

        public int OopNumber
        {
            get => Memory.Read16(0xB97A);
            set => Memory.Write16(0xB97A, value);
        }

        public string OopWord
        {
            get => Memory.ReadString(0xB964);
            set => Memory.WriteString(0xB964, value);
        }

        public int PlayerElement
        {
            get => Memory.Read16(0x7CA0);
            set => Memory.Write16(0x7CA0, value);
        }

        public int PlayerTime { get; set; }

        public bool QuitEngine
        {
            get => Memory.ReadBool(0x7C9D);
            set => Memory.WriteBool(0x7C9D, value);
        }

        public ISoundBufferList SoundBuffer { get; }

        public bool SoundPlaying
        {
            get => Memory.ReadBool(0xD0A8);
            set => Memory.WriteBool(0xD0A8, value);
        }

        public int SoundPriority
        {
            get => Memory.Read16(0xCD9C);
            set => Memory.Write16(0xCD9C, value);
        }

        public int SoundTicks
        {
            get => Memory.Read8(0xCF9D);
            set => Memory.Write8(0xCF9D, value);
        }

        public IReadOnlyList<int> StarChars { get; }

        public int StartBoard
        {
            get => Memory.Read16(0x7CA2);
            set => Memory.Write16(0x7CA2, value);
        }

        public IReadOnlyList<int> TransporterHChars { get; }

        public IReadOnlyList<int> TransporterVChars { get; }

        public IReadOnlyList<int> Vector4 { get; }

        public IReadOnlyList<int> Vector8 { get; }

        public IReadOnlyList<int> WebChars { get; }

        public string WorldFileName
        {
            get => Memory.ReadString(0x2AB6);
            set => Memory.WriteString(0x2AB6, value);
        }

        public bool WorldLoaded
        {
            get => Memory.ReadBool(0xB97C);
            set => Memory.WriteBool(0xB97C, value);
        }
    }
}