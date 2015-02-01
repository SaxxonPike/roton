using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                switch(arg.ToLower())
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
						if (IsMac())
							useOpenGl = false;
						else
							useOpenGl = true;
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
			
		// HACK: I hate this. I really, really hate this. Mono doesn't really
		// leave me any other options (OS X returns PlatformID.Unix instead of
		// PlatformID.MacOSX).
		[DllImport ("libc")]
		static extern int uname(IntPtr buffer);
		static bool IsMac()
		{
			bool isMac = false;
			IntPtr buffer = IntPtr.Zero;

			try
			{
				buffer = Marshal.AllocHGlobal(8192);
				if(uname(buffer) == 0) {
					string osDesc = Marshal.PtrToStringAnsi(buffer);
					if(osDesc == "Darwin")
						isMac = true;
				}
			}
			catch {}
			finally
			{
				if (buffer != IntPtr.Zero)
					Marshal.FreeHGlobal(buffer);
			}
			return isMac;
		}
    }
}
