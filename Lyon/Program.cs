using System;
using Lyon.Dialogs;
using Roton.Interface.Windows;

namespace Lyon
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var openWorldDialog = new OpenWorldDialog();
            if (openWorldDialog.ShowDialog() == FileDialogResult.Ok)
            {
                var game = new Game();
                game.Run(openWorldDialog.FileName);
            }
        }
    }
}