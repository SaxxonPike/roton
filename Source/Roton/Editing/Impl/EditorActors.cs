using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;

namespace Roton.Editing.Impl
{
    public class EditorActors : IEditorActors
    {
        private readonly IEngine _engine;
        private readonly EditorBoard _editorBoard;
        private readonly List<EditorActor> _actors;

        internal EditorActors(IEngine engine, EditorBoard editorBoard)
        {
            _engine = engine;
            _editorBoard = editorBoard;
            _actors = new List<EditorActor>();
            Clear();
        }

        public IEnumerator<IEditorActor> GetEnumerator()
        {
            return _actors
                .Take(_engine.Actors.Count)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEditorActor this[int index] => _actors[index];

        public int Count => _engine.Actors.Count;

        public IEditorActor Add()
        {
            _editorBoard.EnforceBoard();
            var index = _engine.Actors.Count;
            if (index >= _engine.GameSerializer.ActorCapacity - 1)
                return null;

            _engine.State.ActorCount++;
            var actor = this[index];
            actor.CopyFrom(_engine.State.DefaultActor);
            return actor;
        }

        public IEditorActor Spawn(int x, int y, int id, int color)
        {
            _editorBoard.EnforceBoard();
            var index = _engine.Actors.Count;
            if (index >= _engine.GameSerializer.ActorCapacity - 1)
                return null;

            _engine.SpawnActor(new Location(x, y), new Tile(id, color), _engine.ElementList[id].Cycle, null);
            return this[_engine.State.ActorCount];
        }

        public IEditorActor Remove(int index)
        {
            if (index < 0 || index >= _actors.Count)
                return null;

            var actor = _actors[index];
            actor.Index = -1;
            for (var i = index + 1; i < _actors.Count; i++)
                _actors[i].Index--;
            _actors.RemoveAt(index);

            return actor;
        }

        public void Clear()
        {
            foreach (var actor in _actors)
                actor.Index = -1;

            _actors.Clear();
            _actors.AddRange(Enumerable
                .Range(0, _engine.GameSerializer.ActorCapacity)
                .Select(i => new EditorActor(_engine, _editorBoard.EnforceBoard, i))
            );
        }
    }
}