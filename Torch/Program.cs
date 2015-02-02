using System;
using System.Windows.Forms;

namespace Torch
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Editor());
        }

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var mbox = MessageBox.Show(
                "Unhandled exception: " + Environment.NewLine + 
                (e.ExceptionObject as Exception).Message + Environment.NewLine + Environment.NewLine +
                "Copy the stack trace to the clipboard?"
                , "Unhandled Exception",
                MessageBoxButtons.YesNo);
            if (mbox == DialogResult.Yes)
            {
                Clipboard.SetText((e.ExceptionObject as Exception).StackTrace);
            }
        }
    }
}
