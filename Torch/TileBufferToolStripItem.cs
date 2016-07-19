using Roton;
using Roton.WinForms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Roton.Core;

namespace Torch
{
    public partial class TileBufferToolStripItem : ToolStripButton
    {
        public TileBufferToolStripItem()
        {
            InitializeComponent();
            Buffer = new List<Tile>();
            DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            ImageScaling = ToolStripItemImageScaling.None;
            TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            TextImageRelation = TextImageRelation.TextBeforeImage;
            Text = "Buffer";
            AutoSize = true;
        }

        public TileBufferToolStripItem(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public IList<Tile> Buffer { get; private set; }

        public Terminal Terminal { get; set; }
    }
}