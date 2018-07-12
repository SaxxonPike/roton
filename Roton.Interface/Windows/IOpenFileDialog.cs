using Roton.Emulation.Data.Impl;

namespace Roton.Interface.Windows
{
    public interface IOpenFileDialog : IFileDialog
    {
        FileDialogResult ShowDialog(ContextEngine contextEngine);
    }
}
