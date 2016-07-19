using System;
using System.Windows.Forms;

namespace Lyon
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

            foreach (var arg in Environment.GetCommandLineArgs())
            {
                switch (arg.ToLower())
                {
                    case "-opengl":
                        useOpenGl = true;
                        break;
                    case "-winform":
                        useWinForm = true;
                        break;
                }
            }

            // Default to using OpenGL in Mac/Linux if no value was specified.
            if (!useOpenGl && !useWinForm)
            {
                switch (Environment.OSVersion.Platform)
                {
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm(useOpenGl));
        }
    }
}