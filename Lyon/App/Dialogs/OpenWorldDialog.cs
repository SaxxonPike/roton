﻿using System.Windows.Forms;
using Roton.Interface.Windows;

namespace Lyon.App.Dialogs
{
    public class OpenWorldDialog : IOpenFileDialog
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

        public FileDialogResult ShowDialog() => (FileDialogResult) _dialog.ShowDialog();
    }
}