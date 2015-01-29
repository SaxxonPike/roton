using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

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

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            _canDraw = true;
        }

        public IKeyboard Keyboard
        {
            get { throw new NotImplementedException(); }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Plot(int x, int y, AnsiChar ac)
        {
            throw new NotImplementedException();
        }

        public void SetScale(int xScale, int yScale)
        {
            throw new NotImplementedException();
        }

        public void SetSize(int width, int height, bool wide)
        {
            throw new NotImplementedException();
        }

        public void Write(int x, int y, string value, int color)
        {
            throw new NotImplementedException();
        }
    }
}
