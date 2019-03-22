using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Editing.Impl
{
    public class EditorBoards : IEditorBoards
    {
        private readonly IEngine _engine;
        private readonly List<EditorBoard> _boards;

        internal EditorBoards(IEngine engine)
        {
            _engine = engine;
            _boards = new List<EditorBoard>();
            Clear();
        }

        public IEnumerator<IEditorBoard> GetEnumerator()
        {
            return _boards
                .Take(_engine.Boards.Count)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEditorBoard this[int index] => _boards[index];

        public int Count => _engine.Boards.Count;

        public IEditorBoard Add()
        {
            var index = _engine.Boards.Count;
            if (index >= 256)
                return null;

            _engine.World.BoardIndex = index;
            _engine.ClearBoard();
            _engine.PackBoard();
            _engine.UnpackBoard(index);
            return this[index];
        }

        public IEditorBoard Remove(int index)
        {
            if (index < 0 || index >= _boards.Count)
                return null;

            var board = _boards[index];
            board.Index = -1;
            for (var i = index + 1; i < _boards.Count; i++)
                _boards[i].Index--;
            _boards.RemoveAt(index);

            return board;
        }

        public void Clear()
        {
            foreach (var board in _boards)
                board.Index = -1;

            _boards.Clear();
            _boards.AddRange(Enumerable
                .Range(0, 256)
                .Select(i => new EditorBoard(_engine, i))
            );
        }
    }
}