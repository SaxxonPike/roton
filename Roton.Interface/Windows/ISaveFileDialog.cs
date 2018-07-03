using Roton.Core;

namespace Roton.Interface.Windows
{
    public interface ISaveFileDialog : IFileDialog
    {
        FileDialogResult ShowDialog(IWorld worldInfo);
    }
}
