using System.Collections.Generic;

namespace Roton.Editing
{
    public interface IEditorActors : IEnumerable<IEditorActor>
    {
        IEditorActor this[int index] { get; }
        int Count { get; }
        IEditorActor Add();
        IEditorActor Spawn(int x, int y, int elementId, int color);
        IEditorActor Remove(int index);
        void Clear();
    }
}