using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Editing.Impl
{
    public class EditorBoard : IEditorBoard
    {
        private readonly IEngine _engine;

        internal EditorBoard(IEngine engine, int index)
        {
            _engine = engine;
            Index = index;
        }

        internal void EnforceBoard()
        {
            if (_engine.World.BoardIndex != Index)
                _engine.SetBoard(Index);
        }

        private IBoard Board
        {
            get
            {
                EnforceBoard();
                return _engine.Board;
            }
        }

        public int Index { get; set; }

        public IXyPair Camera => Board.Camera;

        public IXyPair Entrance => Board.Entrance;

        public int ExitEast
        {
            get => Board.ExitEast;
            set => Board.ExitEast = value;
        }

        public int ExitNorth
        {
            get => Board.ExitNorth;
            set => Board.ExitNorth = value;
        }

        public int ExitSouth
        {
            get => Board.ExitSouth;
            set => Board.ExitSouth = value;
        }

        public int ExitWest
        {
            get => Board.ExitWest;
            set => Board.ExitWest = value;
        }

        public bool IsDark
        {
            get => Board.IsDark;
            set => Board.IsDark = value;
        }

        public int MaximumShots
        {
            get => Board.MaximumShots;
            set => Board.MaximumShots = value;
        }

        public string Name
        {
            get => _engine.Boards[Index].Name;
            set => _engine.Boards[Index].Name = value;
        }

        public bool RestartOnZap
        {
            get => Board.RestartOnZap;
            set => Board.RestartOnZap = value;
        }

        public int TimeLimit
        {
            get => Board.TimeLimit;
            set => Board.TimeLimit = value;
        }
    }
}