using Roton.Core;
using Roton.Emulation.Data;

namespace Roton.Interface.Windows
{
    public interface ISaveFileDialog : IFileDialog
    {
        FileDialogResult ShowDialog(IWorld worldInfo);
    }
}
