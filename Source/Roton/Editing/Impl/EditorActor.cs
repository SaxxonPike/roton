using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Editing.Impl
{
    public class EditorActor : IEditorActor
    {
        private readonly IEngine _engine;

        internal EditorActor(IEngine engine, Action enforceBoard, int index)
        {
            _engine = engine;
            EnforceBoard = enforceBoard;
            Index = index;
        }

        public Action EnforceBoard { get; }

        private IActor Actor
        {
            get
            {
                EnforceBoard();
                return _engine.Actors[Index];
            }
        }

        public int Index { get; set; }

        public int Instruction
        {
            get => Actor.Instruction;
            set => Actor.Instruction = value;
        }

        public byte[] Code
        {
            get => Actor.Code;
            set => Actor.Code = value;
        }

        public IXyPair Location => Actor.Location;

        public ITile UnderTile => Actor.UnderTile;

        public IXyPair Vector => Actor.Vector;

        public int Cycle
        {
            get => Actor.Cycle;
            set => Actor.Cycle = value;
        }

        public int Follower
        {
            get => Actor.Follower;
            set => Actor.Follower = value;
        }

        public int Leader
        {
            get => Actor.Leader;
            set => Actor.Leader = value;
        }

        public int Length
        {
            get => Actor.Length;
            set => Actor.Length = value;
        }

        public int P1
        {
            get => Actor.P1;
            set => Actor.P1 = value;
        }

        public int P2
        {
            get => Actor.P2;
            set => Actor.P2 = value;
        }

        public int P3
        {
            get => Actor.P3;
            set => Actor.P3 = value;
        }

        public int Pointer
        {
            get => Actor.Pointer;
            set => Actor.Pointer = value;
        }
    }
}