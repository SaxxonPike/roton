using System.Windows.Forms;
using Roton.Interface.Video.Scenes.Composition;

namespace Roton.Interface.Video.Terminals
{
    public interface IEditorTerminal : IGameTerminal
    {
        bool CursorEnabled { get; set; }
        int CursorX { get; set; }
        int CursorY { get; set; }

        void AttachKeyHandler(Form form);

        event MouseEventHandler MouseDown;
        IDirectAccessBitmap RenderAll();
        IDirectAccessBitmap RenderSingle(int character, int color);
    }
}