using System.Windows.Forms;

namespace Roton.Interface.Windows
{
    public class OpenWorldDialog
    {
        private static string Filter
            => string.Join("|",
                "Game Worlds (*.zzt;*.szt)", "*.zzt;*.szt;*.ZZT;*.SZT",
                "ZZT Worlds (*.zzt)", "*.zzt;*.ZZT",
                "Super ZZT Worlds (*.SZT)", "*.szt;*.SZT",
                "Saved Games (*.sav)", "*.sav;*.SAV",
                "All Openable Files (*.zzt;*.szt;*.sav)", "*.zzt;*.szt;*.sav;*.ZZT;*.SZT;*.SAV",
                "All Files (*.*)", "*.*"
                );

        private readonly OpenFileDialog _dialog;

        public OpenWorldDialog()
        {
            _dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                AutoUpgradeEnabled = true,
                Filter = Filter
            };
        }

        public string FileName => _dialog.FileName;

        public DialogResult ShowDialog() => _dialog.ShowDialog();
    }
}
