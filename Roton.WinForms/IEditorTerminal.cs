using System.Drawing;
using System.Windows.Forms;
using Roton.Common;

namespace Roton.WinForms
{
    public interface IEditorTerminal : IGameTerminal
    {
        bool CursorEnabled { get; set; }
        int CursorX { get; set; }
        int CursorY { get; set; }

        void AttachKeyHandler(Form form);

        event MouseEventHandler MouseDown;
        Bitmap RenderSingle(int character, int color);
        Bitmap RenderAll();
    }
}