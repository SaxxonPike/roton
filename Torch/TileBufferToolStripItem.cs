using Roton;
using Roton.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Torch
{
    public partial class TileBufferToolStripItem : ToolStripButton
    {
        public TileBufferToolStripItem()
        {
            InitializeComponent();
            this.Buffer = new List<Tile>();
            this.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            this.ImageScaling = ToolStripItemImageScaling.None;
            this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.Text = "Buffer";
            this.AutoSize = true;
        }

        public TileBufferToolStripItem(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public IList<Tile> Buffer
        {
            get;
            private set;
        }

        public Terminal Terminal
        {
            get;
            set;
        }
    }
}
