using Roton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;

namespace Lyon
{
    public partial class GameForm : Form
    {
        bool initScaleDisplay = true;
        private ITerminal terminal;
        private bool openGL;

        private void CommonSetup()
        {
            // Set up default font and palette.
            var font1 = new Roton.Common.Font();
            var palette1 = new Roton.Common.Palette();

            InitializeComponent();
            InitializeEvents();

            // Load the appropriate terminal.
            if (!openGL)
            {
                terminal = new Roton.Windows.Terminal();
                ((Roton.Windows.Terminal)terminal).TerminalFont = font1;
                ((Roton.Windows.Terminal)terminal).TerminalPalette = palette1;
            }
            else
            {
                terminal = new Roton.OpenGL.Terminal();
                ((Roton.OpenGL.Terminal)terminal).TerminalFont = font1;
                ((Roton.OpenGL.Terminal)terminal).TerminalPalette = palette1;
            }

            terminal.Top = 0;
            terminal.Left = 0;
            terminal.Width = 640;
            terminal.Height = 350;
            terminal.AutoSize = true;
            mainPanel.Controls.Add((UserControl)terminal);

			// Used to help Mono's WinForm implementation set the correct window size
			// before it tries to scale the window.
			this.Width = terminal.Width;
			this.Height = terminal.Height;
        }

        public GameForm(bool openGL = false)
        {
            this.openGL = openGL;
            CommonSetup();
            Initialize(new Context(ContextEngine.ZZT, false));
        }

        public GameForm(Stream source, bool openGL = false)
        {
            this.openGL = openGL;
            CommonSetup();
            Initialize(new Context(source, false));
        }

        public Context Context
        {
            get;
            private set;
        }

        void DumpRam()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    File.WriteAllBytes(sfd.FileName, this.Context.Memory);
                }
            }
        }

        string FileFilters
        {
            get { return "Game Worlds|*.zzt;*.szt|ZZT Worlds|*.zzt|Super ZZT Worlds|*.szt|Saved Games|*.sav|All Openable Files|*.zzt;*.szt;*.sav|All Files|*.*"; }
        }

        void Initialize(Context context)
        {
            speaker.Stop();
            terminal.Keyboard.Clear();
            terminal.Clear();
            if (this.Context is Context)
            {
                this.Context.Stop();
            }

            this.Context = context;
            context.Keyboard = this.terminal.Keyboard;
            context.Speaker = this.speaker;
            context.Terminal = this.terminal;
            UpdateTitle();

            if (initScaleDisplay)
            {
                var workingArea = Screen.FromControl(this).WorkingArea;
                initScaleDisplay = false;
                int idealXScale = (workingArea.Width / this.Width);
                int idealYScale = (workingArea.Height / this.Height);
                int idealScale = Math.Min(idealXScale, idealYScale);
                SetScale(idealScale);
                initScaleDisplay = false;
            }

            context.Start();
        }

        void InitializeEvents()
        {
            exitMenuItem.Click += (object sender, EventArgs e) => { Close(); };
            openWorldMenuItem.Click += (object sender, EventArgs e) => { OpenWorld(); };
            saveWorldToolStripMenuItem.Click += (object sender, EventArgs e) => { SaveWorld(); };
            scale1xMenuItem.Click += (object sender, EventArgs e) => { SetScale(1); };
            scale2xMenuItem.Click += (object sender, EventArgs e) => { SetScale(2); };
            scale3xMenuItem.Click += (object sender, EventArgs e) => { SetScale(3); };
            dumpRAMToolStripMenuItem.Click += (object sender, EventArgs e) => { DumpRam(); };
        }

        void OpenWorld()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = FileFilters;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Initialize(new Context(ofd.FileName, false));
            }
        }

        void RestartCore()
        {
            if (Context != null)
            {
                Initialize(new Context(this.Context.ContextEngine, false));
            }
            else
            {
                RestartZZT();
            }
        }

        void RestartSuperZZT()
        {
            Initialize(new Context(ContextEngine.SuperZZT, false));
        }

        void RestartZZT()
        {
            Initialize(new Context(ContextEngine.ZZT, false));
        }

        void SaveWorld()
        {
            if (Context != null)
            {
                var sfd = new SaveFileDialog();
                sfd.Filter = FileFilters;
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Initialize(new Context(sfd.FileName, false));
                    Context.Save(sfd.FileName);
                }
            }
        }

        void SetScale(int scale)
        {
            terminal.SetScale(scale, scale);
        }

        void UpdateTitle()
        {
            this.Text = "Lyon" + (string.IsNullOrWhiteSpace(Context.WorldData.Name) ? "" : " [" + Context.WorldData.Name + "]");
        }
    }
}
