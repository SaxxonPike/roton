using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Editing.Impl
{
    public class Editor : IEditor
    {
        public Editor(IEngine engine)
        {
            Boards = new EditorBoards(engine);
        }
    
        public IEditorBoards Boards { get; }
    }
}