using Roton.Common;
using System;
using System.IO;
using System.Windows.Forms;
using Roton.Core;

namespace Lyon
{
    public partial class GameForm : Form
    {
        private bool _initScaleDisplay = true;
        private IGameTerminal _terminal;
        private bool _openGl;

        private void CommonSetup()
        {
            // Set up default font and palette.
            var font1 = new Font();
            var palette1 = new Palette();

            InitializeComponent();
            InitializeEvents();

            // Load the appropriate terminal.
            if (!_openGl)
                _terminal = new Roton.WinForms.Terminal();
            else
                _terminal = new Roton.WinForms.OpenGL.Terminal();

            _terminal.Top = 0;
            _terminal.Left = 0;
            _terminal.Width = 640;
            _terminal.Height = 350;
            _terminal.AutoSize = true;
            _terminal.TerminalFont = font1;
            _terminal.TerminalPalette = palette1;
            mainPanel.Controls.Add((UserControl) _terminal);

            // Used to help Mono's WinForm implementation set the correct window size
            // before it tries to scale the window.
            Width = _terminal.Width;
            Height = _terminal.Height;
        }

        public GameForm(bool openGl = false)
        {
            _openGl = openGl;
            CommonSetup();
            Initialize(new Context(ContextEngine.Zzt, false));
        }

        public GameForm(Stream source, bool openGl = false)
        {
            _openGl = openGl;
            CommonSetup();
            Initialize(new Context(source, false));
        }

        public Context Context { get; private set; }

        private void DumpRam()
        {
            using (var sfd = new SaveFileDialog())
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(sfd.FileName, Context.Memory);
                }
            }
        }

        private string FileFilters
            =>
                "Game Worlds (*.zzt;*.szt)|*.zzt;*.szt;*.ZZT;*.SZT|ZZT Worlds (*.zzt)|*.zzt;*.ZZT|Super ZZT Worlds (*.SZT)|*.szt;*.SZT|Saved Games (*.sav)|*.sav;*.SAV|All Openable Files (*.zzt;*.szt;*.sav)|*.zzt;*.szt;*.sav;*.ZZT;*.SZT;*.SAV|All Files (*.*)|*.*"
            ;

        private void Initialize(Context context)
        {
            speaker.Stop();
            _terminal.Keyboard.Clear();
            _terminal.Clear();
            Context?.Stop();

            Context = context;
            context.Keyboard = _terminal.Keyboard;
            context.Speaker = speaker;
            context.Terminal = _terminal;
            UpdateTitle();

            if (_initScaleDisplay)
            {
                var workingArea = Screen.FromControl(this).WorkingArea;
                _initScaleDisplay = false;
                var idealXScale = workingArea.Width/Width;
                var idealYScale = workingArea.Height/Height;
                var idealScale = Math.Min(idealXScale, idealYScale);
                SetScale(idealScale);
                _initScaleDisplay = false;
            }

            context.Start();
        }

        private void InitializeEvents()
        {
            exitMenuItem.Click += (sender, e) => { Close(); };
            openWorldMenuItem.Click += (sender, e) => { OpenWorld(); };
            saveWorldToolStripMenuItem.Click += (sender, e) => { SaveWorld(); };
            scale1xMenuItem.Click += (sender, e) => { SetScale(1); };
            scale2xMenuItem.Click += (sender, e) => { SetScale(2); };
            scale3xMenuItem.Click += (sender, e) => { SetScale(3); };
            dumpRAMToolStripMenuItem.Click += (sender, e) => { DumpRam(); };
        }

        private void OpenWorld()
        {
            var ofd = new OpenFileDialog {Filter = FileFilters};
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Initialize(new Context(ofd.FileName, false));
            }
        }

        private void SaveWorld()
        {
            if (Context != null)
            {
                var sfd = new SaveFileDialog {Filter = FileFilters};
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //Initialize(new Context(sfd.FileName, false));
                    Context.Save(sfd.FileName);
                }
            }
        }

        private void SetScale(int scale)
        {
            _terminal.SetScale(scale, scale);
        }

        private void UpdateTitle()
        {
            Text = "Lyon" +
                   (string.IsNullOrWhiteSpace(Context.WorldData.Name) ? "" : " [" + Context.WorldData.Name + "]");
        }
    }
}