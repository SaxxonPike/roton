﻿using System;
using System.IO;
using System.Windows.Forms;
using Roton.Common;
using Roton.Core;
using Roton.FileIo;
using Roton.WinForms;

namespace Lyon
{
    public partial class GameForm : Form
    {
        private readonly bool _openGl;
        private bool _initScaleDisplay = true;
        private IGameTerminal _terminal;

        public GameForm(bool openGl = false)
        {
            _openGl = openGl;
            CommonSetup();
            Shown += (sender, e) => { Initialize(new Context(GetCoreConfiguration(), ContextEngine.Zzt)); };
        }

        public GameForm(Stream source, bool openGl = false)
        {
            _openGl = openGl;
            CommonSetup();
            Shown += (sender, e) => { Initialize(new Context(GetCoreConfiguration(), source)); };
        }

        private string FileFilters
            =>
                "Game Worlds (*.zzt;*.szt)|*.zzt;*.szt;*.ZZT;*.SZT|ZZT Worlds (*.zzt)|*.zzt;*.ZZT|Super ZZT Worlds (*.SZT)|*.szt;*.SZT|Saved Games (*.sav)|*.sav;*.SAV|All Openable Files (*.zzt;*.szt;*.sav)|*.zzt;*.szt;*.sav;*.ZZT;*.SZT;*.SAV|All Files (*.*)|*.*"
            ;

        private IContext Context { get; set; }

        private void CommonSetup()
        {
            // Set up default font and palette.
            var font1 = new RasterFont();
            var palette1 = new Palette();

            InitializeComponent();
            InitializeEvents();

            // Load the appropriate terminal.
            if (!_openGl)
                _terminal = new Terminal();
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

        private void DumpRam()
        {
            using (var sfd = new SaveFileDialog())
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(sfd.FileName, Context.DumpMemory());
                }
            }
        }

        private IEngineConfiguration GetCoreConfiguration()
        {
            return new EngineConfiguration
            {
                Disk = new DiskFileSystem(),
                EditorMode = false,
                Keyboard = _terminal.Keyboard,
                Speaker = speaker,
                Terminal = _terminal
            };
        }

        private void Initialize(IContext context)
        {
            speaker.Stop();
            _terminal.Keyboard.Clear();
            _terminal.Clear();
            Context?.Stop();

            Context = context;
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
                Initialize(new Context(GetCoreConfiguration(), File.ReadAllBytes(ofd.FileName)));
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
                   (string.IsNullOrWhiteSpace(Context.WorldData.Name) ? string.Empty : " [" + Context.WorldData.Name + "]");
        }
    }
}