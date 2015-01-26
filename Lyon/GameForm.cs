﻿using Roton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lyon
{
    public partial class GameForm : Form
    {
        bool initScaleDisplay = true;

        public GameForm()
        {
            InitializeComponent();
            InitializeEvents();
            Initialize(new Context(ContextEngine.ZZT, false));
        }

        public GameForm(Stream source)
        {
            InitializeComponent();
            InitializeEvents();
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
            scale1xMenuItem.Click += (object sender, EventArgs e) => { SetScale(1); };
            scale2xMenuItem.Click += (object sender, EventArgs e) => { SetScale(2); };
            scale3xMenuItem.Click += (object sender, EventArgs e) => { SetScale(3); };
            mainPanel.Resize += (object sender, EventArgs e) => { UpdateTerminalLocation(); };
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

        void SetScale(int scale)
        {
            terminal.SetScale(scale, scale);
            UpdateTerminalLocation();
        }

        void UpdateTerminalLocation()
        {
            terminal.Top = (mainPanel.Height - terminal.Height) / 2;
            terminal.Left = (mainPanel.Width - terminal.Width) / 2;
        }

        void UpdateTitle()
        {
            this.Text = "Lyon" + (string.IsNullOrWhiteSpace(Context.WorldData.Name) ? "" : " [" + Context.WorldData.Name + "]");
        }
    }
}