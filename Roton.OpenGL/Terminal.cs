using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Roton;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;

namespace Roton.OpenGL {
    public partial class Terminal : UserControl, ITerminal
    {
        private bool _canDraw = false;

        public Terminal()
        {
            InitializeComponent();

            // Initialize the GLControl late so that the WinForm designer
            // doesn't crash.
            var glControl = new GLControl {Dock = DockStyle.Fill};
            glControl.Load += glControl_Load;
        }

        void glControl_Load(object sender, EventArgs e)
        {
            _canDraw = true;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void Plot(int x, int y, AnsiChar ac)
        {
            throw new System.NotImplementedException();
        }

        public void SetSize(int width, int height, bool wide)
        {
            throw new System.NotImplementedException();
        }

        public void Write(int x, int y, string value, int color)
        {
            throw new System.NotImplementedException();
        }
    }
}
