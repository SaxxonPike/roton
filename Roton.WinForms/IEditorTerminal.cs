using Roton.Common;
using System.Drawing;
using System.Windows.Forms;

namespace Roton.WinForms
{
    public interface IEditorTerminal : IGameTerminal
    {
        bool CursorEnabled { get; set; }
        int CursorX { get; set; }
        int CursorY { get; set; }

        void AttachKeyHandler(Form form);
        Bitmap RenderSingle(int character, int color);

        event MouseEventHandler MouseDown;
    }
}