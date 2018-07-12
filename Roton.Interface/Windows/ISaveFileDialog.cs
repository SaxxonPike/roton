using Roton.Emulation.Data.Impl;

namespace Roton.Interface.Windows
{
    public interface ISaveFileDialog : IFileDialog
    {
        FileDialogResult ShowDialog(ContextEngine contextEngine, string worldName, bool isLocked);
    }
}
