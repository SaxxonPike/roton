using System;
using System.IO;
using System.Windows.Forms;
using Lyon.Dialogs;

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
            if (openWorldDialog.ShowDialog() == DialogResult.OK)
            {
                var game = new Game();
                game.Run(openWorldDialog.FileName);
            }
        }
    }
}