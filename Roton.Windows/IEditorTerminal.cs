using System.Drawing;
using System.Windows.Forms;

namespace Roton.Windows {
    public interface IEditorTerminal : ITerminal {
        bool CursorEnabled { get; set; }
        int CursorX { get; set; }
        int CursorY { get; set; }

        void AttachKeyHandler(Form form);
        Bitmap RenderSingle(int character, int color);

        event MouseEventHandler MouseDown;
    }
}
