using System.Drawing;
using System.Windows.Forms;

namespace Roton.Interface.Video.Controls
{
    public interface IEditorTerminal : IGameTerminal
    {
        bool CursorEnabled { get; set; }
        int CursorX { get; set; }
        int CursorY { get; set; }

        void AttachKeyHandler(Form form);

        event MouseEventHandler MouseDown;
        Bitmap RenderAll();
        Bitmap RenderSingle(int character, int color);
    }
}