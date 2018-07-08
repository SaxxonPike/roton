using System.Windows.Forms;
using Roton.Core;
using Roton.Emulation.Data;
using Roton.Interface.Windows;

namespace Lyon.App.Dialogs
{
    public class SaveWorldDialog : ISaveFileDialog
    {
        private static string UnknownFilter
            => string.Join("|",
                "All Files (*.*)", "*.*"
                );

        private static string ZztWorldFilter
            => string.Join("|",
                "ZZT Worlds (*.zzt)", "*.zzt;*.ZZT",
                "All Files (*.*)", "*.*"
                );

        private static string SuperZztWorldFilter
            => string.Join("|",
                "Super ZZT Worlds (*.SZT)", "*.szt;*.SZT",
                "All Files (*.*)", "*.*"
                );

        private static string SavedGameFilter
            => string.Join("|",
                "Saved Games (*.sav)", "*.sav;*.SAV",
                "All Files (*.*)", "*.*"
                );

        private readonly SaveFileDialog _dialog;

        public SaveWorldDialog()
        {
            _dialog = new SaveFileDialog
            {
                AutoUpgradeEnabled = true,
                OverwritePrompt = true
            };
        }

        public string FileName => _dialog.FileName;

        public FileDialogResult ShowDialog(IWorld worldInfo)
        {
            if (worldInfo.IsLocked)
            {
                _dialog.Filter = SavedGameFilter;
            }
            else
            {
                switch (worldInfo.WorldType)
                {
                    case -1:
                        _dialog.Filter = ZztWorldFilter;
                        break;
                    case -2:
                        _dialog.Filter = SuperZztWorldFilter;
                        break;
                    default:
                        _dialog.Filter = UnknownFilter;
                        break;
                }
            }
            return (FileDialogResult) _dialog.ShowDialog();
        }
    }
}
