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
        private readonly Lazy<IEngineResourceService> _engineResourceService;
        private readonly Lazy<IHeap> _heap;
        private readonly Memory<byte> _memory;

        public SuperState(IMemory memory, Lazy<IEngineResourceService> engineResourceService, Lazy<IHeap> heap)
        {
            _engineResourceService = engineResourceService;
            _heap = heap;

            _memory = memory.Slice(0);
            
            memory.Write(0x0000, EngineResourceService.GetMemoryData());
            BorderTile = new Tile(0, 0); // Not in memory
            DefaultActor = new Actor(memory, Heap, 0x2262);
            EdgeTile = new MemoryTile(memory, 0x2260);
            KeyVector = new MemoryVector(memory, 0xCC6E);
            LineChars = new ByteString(memory, 0x22BA);
            SoundBuffer = new SoundBufferList(memory, 0xCF9E);
            StarChars = new ByteString(memory, 0x2064);
            TransporterHChars = new ByteString(memory, 0x1F64);
            TransporterVChars = new ByteString(memory, 0x1E64);
            Vector4 = new Int16List(memory, 0x2250, 8);
            Vector8 = new Int16List(memory, 0x2230, 16);
            WebChars = new ByteString(memory, 0x227C);
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
            get => _memory.Read16(0xB95A);
            set => _memory.Write16(0xB95A, value);
        }

        public int ActorCount
        {
            get => _memory.Read16(0x6AB3);
            set => _memory.Write16(0x6AB3, value);
        }

        public int BoardCount
        {
            get => _memory.Read16(0x7784);
            set => _memory.Write16(0x7784, value);
        }

        public ITile BorderTile { get; }

        public bool BreakGameLoop
        {
            get => _memory.ReadBool(0x7C9E);
            set => _memory.WriteBool(0x7C9E, value);
        }

        public bool CancelScroll { get; set; }

        public IActor DefaultActor { get; }

        public string DefaultBoardName
        {
            get => _memory.ReadString(0x2B32);
            set => _memory.WriteString(0x2B32, value);
        }

        public string DefaultSaveName
        {
            get => _memory.ReadString(0x2AF4);
            set => _memory.WriteString(0x2AF4, value);
        }

        public string DefaultWorldName
        {
            get => _memory.ReadString(0x2B70);
            set => _memory.WriteString(0x2B70, value);
        }

        public ITile EdgeTile { get; }

        public bool EditorMode
        {
            get => _memory.ReadBool(0xB960);
            set => _memory.WriteBool(0xB960, value);
        }

        public int ForestIndex
        {
            get => _memory.Read16(0x2334);
            set => _memory.Write16(0x2334, value);
        }

        public int GameCycle
        {
            get => _memory.Read16(0xB958);
            set => _memory.Write16(0xB958, value);
        }

        public bool GameOver
        {
            get => _memory.ReadBool(0xCD9B);
            set => _memory.WriteBool(0xCD9B, value);
        }

        public bool GamePaused
        {
            get => _memory.ReadBool(0xB95C);
            set => _memory.WriteBool(0xB95C, value);
        }

        public bool GameQuiet
        {
            get => _memory.ReadBool(0xCD9A);
            set => _memory.WriteBool(0xCD9A, value);
        }

        public int GameSpeed
        {
            get => _memory.Read16(0x7CA4);
            set => _memory.Write16(0x7CA4, value);
        }

        public int GameWaitTime
        {
            get => _memory.Read16(0xB956);
            set => _memory.Write16(0xB956, value);
        }

        public bool Init { get; set; }

        public bool KeyArrow
        {
            get => _memory.ReadBool(0xCC84);
            set => _memory.WriteBool(0xCC84, value);
        }

        public EngineKeyCode KeyPressed
        {
            get => (EngineKeyCode) _memory.Span[0xCC76];
            set => _memory.Write8(0xCC76, (int) value);
        }

        public bool KeyShift
        {
            get => _memory.ReadBool(0xCC72);
            set => _memory.WriteBool(0xCC72, value);
        }

        public IXyPair KeyVector { get; }

        public IReadOnlyList<int> LineChars { get; }

        public string Message
        {
            get => _memory.ReadString(0x7C22);
            set => _memory.WriteString(0x7C22, value);
        }

        public string Message2
        {
            get => _memory.ReadString(0x7C60);
            set => _memory.WriteString(0x7C60, value);
        }

        public int OopByte
        {
            get => _memory.Span[0xB962];
            set => _memory.Write8(0xB962, value);
        }

        public int OopNumber
        {
            get => _memory.Read16(0xB97A);
            set => _memory.Write16(0xB97A, value);
        }

        public string OopWord
        {
            get => _memory.ReadString(0xB964);
            set => _memory.WriteString(0xB964, value);
        }

        public int PlayerElement
        {
            get => _memory.Read16(0x7CA0);
            set => _memory.Write16(0x7CA0, value);
        }

        public int PlayerTime { get; set; }

        public bool QuitEngine
        {
            get => _memory.ReadBool(0x7C9D);
            set => _memory.WriteBool(0x7C9D, value);
        }

        public ISoundBufferList SoundBuffer { get; }

        public bool SoundPlaying
        {
            get => _memory.ReadBool(0xD0A8);
            set => _memory.WriteBool(0xD0A8, value);
        }

        public int SoundPriority
        {
            get => _memory.Read16(0xCD9C);
            set => _memory.Write16(0xCD9C, value);
        }

        public int SoundTicks
        {
            get => _memory.Span[0xCF9D];
            set => _memory.Write8(0xCF9D, value);
        }

        public IReadOnlyList<int> StarChars { get; }

        public int StartBoard
        {
            get => _memory.Read16(0x7CA2);
            set => _memory.Write16(0x7CA2, value);
        }

        public IReadOnlyList<int> TransporterHChars { get; }

        public IReadOnlyList<int> TransporterVChars { get; }

        public IReadOnlyList<int> Vector4 { get; }

        public IReadOnlyList<int> Vector8 { get; }

        public IReadOnlyList<int> WebChars { get; }

        public string WorldFileName
        {
            get => _memory.ReadString(0x2AB6);
            set => _memory.WriteString(0x2AB6, value);
        }

        public bool WorldLoaded
        {
            get => _memory.ReadBool(0xB97C);
            set => _memory.WriteBool(0xB97C, value);
        }
    }
}