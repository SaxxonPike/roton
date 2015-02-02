using System;
using System.Diagnostics;
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
            // TODO: Make argument/config handling better.
            var useOpenGl = false;
            var useWinForm = false;

            foreach(var arg in Environment.GetCommandLineArgs()) {
                switch(arg.ToLower()) {
                    case "-opengl":
                        useOpenGl = true;
                        break;
                    case "-winform":
                        useWinForm = true;
                        break;
                }
            }

            // Default to using OpenGL in Mac/Linux if no value was specified.
            if(!useOpenGl && !useWinForm) {
                switch(Environment.OSVersion.Platform) {
                    case PlatformID.MacOSX:
                    case PlatformID.Unix:
                        useOpenGl = true;
                        useWinForm = false;
                        break;
                    default:
                        useOpenGl = false;
                        useWinForm = true;
                        break;
                }
            }

            if(!Debugger.IsAttached)
                AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Editor(useOpenGl));
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
