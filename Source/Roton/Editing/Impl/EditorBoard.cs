using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Editing.Impl
{
    public class EditorBoard : IEditorBoard
    {
        private readonly IEngine _engine;

        public EditorBoard(IEngine engine, int index)
        {
            _engine = engine;
            Index = index;
        }

        private void EnforceBoard(int index)
        {
            if (_engine.World.BoardIndex != index)
                _engine.SetBoard(index);
        }

        public int Index { get; set; }

        public IXyPair Camera
        {
            get
            {
                EnforceBoard(Index);
                return _engine.Board.Camera;
            }
        }

        public IXyPair Entrance
        {
            get
            {
                EnforceBoard(Index);
                return _engine.Board.Entrance;
            }
        }

        public int ExitEast
        {
            get
            {
                EnforceBoard(Index);
                return _engine.Board.ExitEast;
            }
            set
            {
                EnforceBoard(Index);
                _engine.Board.ExitEast = value;
            }
        }

        public int ExitNorth
        {
            get
            {
                EnforceBoard(Index);
                return _engine.Board.ExitNorth;
            }
            set
            {
                EnforceBoard(Index);
                _engine.Board.ExitNorth = value;
            }
        }

        public int ExitSouth
        {
            get
            {
                EnforceBoard(Index);
                return _engine.Board.ExitSouth;
            }
            set
            {
                EnforceBoard(Index);
                _engine.Board.ExitSouth = value;
            }
        }

        public int ExitWest
        {
            get
            {
                EnforceBoard(Index);
                return _engine.Board.ExitWest;
            }
            set
            {
                EnforceBoard(Index);
                _engine.Board.ExitWest = value;
            }
        }

        public bool IsDark
        {
            get
            {
                EnforceBoard(Index);
                return _engine.Board.IsDark;
            }
            set
            {
                EnforceBoard(Index);
                _engine.Board.IsDark = value;
            }
        }

        public int MaximumShots
        {
            get
            {
                EnforceBoard(Index);
                return _engine.Board.MaximumShots;
            }
            set
            {
                EnforceBoard(Index);
                _engine.Board.MaximumShots = value;
            }
        }

        public string Name
        {
            get => _engine.Boards[Index].Name;
            set => _engine.Boards[Index].Name = value;
        }

        public bool RestartOnZap
        {
            get
            {
                EnforceBoard(Index);
                return _engine.Board.RestartOnZap;
            }
            set
            {
                EnforceBoard(Index);
                _engine.Board.RestartOnZap = value;
            }
        }

        public int TimeLimit
        {
            get
            {
                EnforceBoard(Index);
                return _engine.Board.TimeLimit;
            }
            set
            {
                EnforceBoard(Index);
                _engine.Board.TimeLimit = value;
            }
        }
    }
}