using System.Collections.Generic;

namespace Roton.Editing
{
    public interface IEditorBoards : IEnumerable<IEditorBoard>
    {
        IEditorBoard this[int index] { get; }
        int Count { get; }
        IEditorBoard Add();
        IEditorBoard Remove(int index);
        void Clear();
    }
}