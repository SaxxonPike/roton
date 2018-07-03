namespace Roton.Interface.Windows
{
    public interface IOpenFileDialog : IFileDialog
    {
        FileDialogResult ShowDialog();
    }
}
